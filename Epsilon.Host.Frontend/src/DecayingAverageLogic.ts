import {
    LearningDomain,
    LearningDomainOutcomeRecord,
    LearningDomainSubmission,
} from "@/api"

export interface DecayingAveragePerActivity {
    outcome: number
    activity: string
    decayingAverage: number
}

export interface DecayingAveragePerLayer {
    architectureLayer: string
    layerActivities: DecayingAveragePerActivity[]
}

export interface DecayingAveragePerSkill {
    skill: string | null
    decayingAverage: number | null
    masteryLevel: number | null
}

export class DecayingAverageLogic {
    private static getAllOutcomes(
        submissions: LearningDomainSubmission[]
    ): LearningDomainOutcomeRecord[] {
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
    public static getAverageSkillOutcomeScores(
        submissions: LearningDomainSubmission[],
        domain: LearningDomain
    ): DecayingAveragePerSkill[] {
        const listOfResults = Object.entries(
            this.groupBy(
                this.getAllOutcomes(submissions),
                (r) => r.outcome?.id as unknown as string
            )
        ).map(([, j]) => {
            return {
                skill: j.at(0)?.outcome?.row?.id,
                masteryLevel: j
                    ?.sort(
                        (a) => a.outcome?.value?.shortName as never as number
                    )
                    ?.at(0)?.outcome?.value?.shortName as unknown as number,
                decayingAverage: this.getDecayingAverageFromOneOutcomeType(j),
            } as unknown as DecayingAveragePerSkill
        })
        return domain.rowsSet?.types?.map((s) => {
            let score = 0.0
            const filteredResults = listOfResults.filter(
                (r) => r.skill === s.id
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
            } as DecayingAveragePerSkill
        }) as DecayingAveragePerSkill[]
    }

    /**
     * Calculate the averages for each task type divided in architecture layers.
     * @param submissions
     * @param domain
     * @constructor
     */
    public static getAverageTaskOutcomeScores(
        submissions: LearningDomainSubmission[],
        domain: LearningDomain
    ): DecayingAveragePerLayer[] {
        const canvasDecaying = this.getDecayingAverageForAllOutcomes(
            submissions,
            domain
        )
        return domain.rowsSet?.types?.map((layer) => {
            return {
                architectureLayer: layer.id,
                layerActivities: domain.columnsSet?.types?.map((activity) => {
                    let totalScoreActivity = 0
                    let totalScoreArchitectureActivity = 0
                    let amountOfActivities = 0

                    //Calculate the total score from activity
                    canvasDecaying.map((l) =>
                        l.layerActivities
                            ?.filter((la) => la.activity === activity.id)
                            .map(
                                (la) =>
                                    (totalScoreActivity +=
                                        la.decayingAverage &&
                                        amountOfActivities++)
                            )
                    )

                    //Calculate the total score from activity inside this architecture layer
                    canvasDecaying
                        .filter((l) => l.architectureLayer === layer.id)
                        .map((l) =>
                            l.layerActivities
                                ?.filter((la) => la.activity === activity.id)
                                .map((la) => {
                                    totalScoreArchitectureActivity +=
                                        la.decayingAverage
                                })
                        )
                    return {
                        activity: activity.id,
                        decayingAverage:
                            ((totalScoreActivity / amountOfActivities) *
                                totalScoreArchitectureActivity) /
                            totalScoreActivity,
                    } as unknown as DecayingAveragePerActivity
                }),
            }
        }) as unknown as DecayingAveragePerLayer[]
    }

    /**
     * Calculate average of given tasks divided in architectural layers
     * @param submissions
     * @param domain
     * @constructor
     * @private
     */
    private static getDecayingAverageForAllOutcomes(
        submissions: LearningDomainSubmission[],
        domain: LearningDomain
    ): DecayingAveragePerLayer[] {
        const results = this.getAllOutcomes(submissions)
        return domain.rowsSet?.types?.map((l) => {
            return {
                architectureLayer: l.id,
                layerActivities: Object.entries(
                    this.groupBy(
                        results.filter(
                            (layer) => layer.outcome?.row?.id === l.id
                        ),
                        (r) => r.outcome?.id as unknown as string
                    )
                ).map(([i, j]) => {
                    //From all selected outcomes calculate the decaying average, Give outcome id and activity layer.
                    return {
                        outcome: i,
                        activity: j.at(0)?.outcome?.column?.id,
                        decayingAverage:
                            this.getDecayingAverageFromOneOutcomeType(j),
                    }
                }) as unknown as DecayingAveragePerActivity[],
            }
        }) as unknown as DecayingAveragePerLayer[]
    }

    /**
     * Calculate decaying average described by Canvas: https://community.canvaslms.com/t5/Canvas-Basics-Guide/What-are-Outcomes/ta-p/75#decaying_average
     * @param results
     * @constructor
     * @private
     */
    private static getDecayingAverageFromOneOutcomeType(
        results: LearningDomainOutcomeRecord[]
    ): number {
        let totalGradeScore = 0.0
        const recentResult = results.reverse().pop()
        if (recentResult && recentResult.grade) {
            if (results.length > 0) {
                results.forEach(
                    (r) => (totalGradeScore += r.grade ? r.grade : 0)
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

    private static groupBy<T>(
        arr: T[],
        fn: (item: T) => number | string
    ): Record<string, T[]> {
        return arr.reduce<Record<string, T[]>>((prev, curr) => {
            const groupKey = fn(curr)
            const group = prev[groupKey] || []
            group.push(curr)
            return { ...prev, [groupKey]: group }
        }, {})
    }
}
