import {
    type LearningDomain,
    type LearningDomainOutcomeRecord,
    type LearningDomainSubmission,
} from "~/api.generated"

export interface DecayingAveragePerActivity {
    outcome: number;
    activity: string;
    decayingAverage: number;
}

export interface DecayingAveragePerLayer {
    architectureLayer: string;
    layerActivities: DecayingAveragePerActivity[];
}

export interface DecayingAveragePerSkill {
    skill: string | null;
    decayingAverage: number | null;
    masteryLevel: number | null;
}

/**
 * Get a list of all outcomes that have been submitted with a submission
 * @param submissions
 * @private
 */
export const getAllOutcomes = (
    submissions: LearningDomainSubmission[],
): LearningDomainOutcomeRecord[] => {
    const list: LearningDomainOutcomeRecord[] = []
    submissions.map((s) => {
        if (s.results) {
            s.results.map((outcome) => list.push(outcome))
        }
    })
    return list
}

/**
 * Calculate the averages for each skill type
 * @param submissions
 * @param domain
 * @constructor
 */
export const getAverageSkillOutcomeScores = (
    submissions: LearningDomainSubmission[],
    domain: LearningDomain,
): DecayingAveragePerSkill[] => {
    const listOfResults = Object.entries(
        groupBy(
            getAllOutcomes(submissions),
            (r) => r.outcome?.id,
        ),
    ).map(([, j]) => {
        return {
            skill: j.at(0)?.outcome?.row.id,
            masteryLevel: j
                .sort((a) => a.outcome?.value.shortName)
                .at(0)?.outcome?.value?.shortName,
            decayingAverage: getDecayingAverageFromOneOutcomeType(j),
        }
    })

    return domain.rowsSet?.types?.map((s) => {
        let score = 0.0

        const filteredResults = listOfResults.filter(
            (r) => r.skill === s.id,
        )

        filteredResults.map((result) => {
            if (result.decayingAverage) {
                score += result.decayingAverage
            }
        })

        return {
            skill: s.id,
            masteryLevel: filteredResults
                .sort((a) => a.masteryLevel as never as number)
                .at(filteredResults.length - 1)?.masteryLevel,
            decayingAverage: score / filteredResults.length,
        }
    })
}

/**
 * Calculate the averages for each task type divided in architecture layers.
 * @param submissions
 * @param domain
 * @constructor
 */
export const getAverageTaskOutcomeScores = (
    submissions: LearningDomainSubmission[],
    domain: LearningDomain,
): DecayingAveragePerLayer[] => {
    const canvasDecaying = getDecayingAverageForAllOutcomes(
        submissions,
        domain,
    )
    return domain.rowsSet?.types?.map((layer) => {
        return {
            architectureLayer: layer.id,
            layerActivities: domain.columnsSet?.types.map((activity) => {
                let totalScoreActivity = 0
                let totalScoreArchitectureActivity = 0
                let amountOfActivities = 0

                //Calculate the total score from activity
                canvasDecaying.map((l) =>
                    l.layerActivities
                        .filter((la) => la.activity === activity.id)
                        .map(
                            (la) =>
                                (totalScoreActivity +=
                                    la.decayingAverage &&
                                    amountOfActivities++),
                        ),
                )

                //Calculate the total score from activity inside this architecture layer
                canvasDecaying
                    .filter((l) => l.architectureLayer === layer.id)
                    .map((l) =>
                        l.layerActivities
                            .filter((la) => la.activity === activity.id)
                            .map((la) => {
                                totalScoreArchitectureActivity +=
                                    la.decayingAverage
                            }),
                    )
                return {
                    activity: activity.id,
                    decayingAverage:
                        ((totalScoreActivity / amountOfActivities) *
                            totalScoreArchitectureActivity) /
                        totalScoreActivity,
                }
            }),
        }
    })
}

/**
 * Calculate average of given tasks divided in architectural layers
 * @param submissions
 * @param domain
 * @constructor
 * @private
 */
export const getDecayingAverageForAllOutcomes = (
    submissions: LearningDomainSubmission[],
    domain: LearningDomain,
): DecayingAveragePerLayer[] => {
    const results = getAllOutcomes(submissions)

    return domain.rowsSet.types.map((l) => {
        return {
            architectureLayer: l.id,
            layerActivities: Object.entries(
                groupBy(
                    results.filter(
                        (layer) => layer.outcome?.row.id === l.id,
                    ),
                    (r) => r.outcome?.id,
                ),
            ).map(([i, j]) => {
                //From all selected outcomes calculate the decaying average, Give outcome id and activity layer.
                return {
                    outcome: i,
                    activity: j.at(0)?.outcome?.column?.id,
                    decayingAverage: getDecayingAverageFromOneOutcomeType(j),
                }
            }),
        }
    })
}

/**
 * Calculate decaying average described by Canvas: https://community.canvaslms.com/t5/Canvas-Basics-Guide/What-are-Outcomes/ta-p/75#decaying_average
 * @param results
 * @constructor
 * @private
 */
export const getDecayingAverageFromOneOutcomeType = (
    results: LearningDomainOutcomeRecord[],
): number => {
    let totalGradeScore = 0.0
    const recentResult = results.reverse().pop()
    if (recentResult && recentResult.grade) {
        if (results.length > 0) {
            results.forEach(
                (r) => (totalGradeScore += r.grade ? r.grade : 0),
            )
            totalGradeScore =
                (totalGradeScore / results.length) * 0.35 +
                recentResult.grade * 0.65
        } else {
            totalGradeScore = recentResult.grade
        }
    }
    return totalGradeScore
}

/**
 * Group a given array with object to a record with a key and an array with values.
 * @param arr
 * @param fn
 * @private
 */
export const groupBy = <T>(
    arr: T[],
    fn: (item: T) => number | string,
): Record<string, T[]> => {
    return arr.reduce<Record<string, T[]>>((previous, current) => {
        const groupKey = fn(current)
        const group = previous[groupKey] || []
        group.push(current)
        return { ...previous, [groupKey]: group }
    }, {})
}