import {
	type LearningDomain,
	type LearningDomainOutcomeRecord,
	type LearningDomainSubmission,
} from "~/api.generated"

export interface DecayingAveragePerActivity {
	outcome: string
	activity: string
	decayingAverage: number
}

export interface DecayingAveragePerLayer {
	architectureLayer: string
	layerActivities: DecayingAveragePerActivity[]
}

export interface DecayingAveragePerSkill {
	skill: string
	decayingAverage: number
	masteryLevel: number | null
}

/**
 * Get a list of all outcomes that have been submitted with a submission
 * @param submissions
 * @private
 */
export const getAllOutcomes = (
	submissions: LearningDomainSubmission[]
): LearningDomainOutcomeRecord[] => submissions.flatMap((a) => a.results!)

/**
 * Calculate the averages for each skill type
 * @param submissions
 * @param domain
 * @constructor
 */
export const calculateAverageSkillOutcomes = (
	submissions: LearningDomainSubmission[],
	domain: LearningDomain
): DecayingAveragePerSkill[] => {
	const outcomes = getAllOutcomes(submissions)

	const groupedOutcomes = Object.entries(
		groupBy(outcomes, (outcome) => outcome.outcome?.id!)
	)

	const listOfResults = groupedOutcomes.map(([, outcomesGroup]) => ({
		decayingAverage: calculateDecayingAverageForOutcomeType(outcomesGroup),
		masteryLevel: outcomesGroup
			.sort((a, b) =>
				(a.outcome?.value.shortName || "").localeCompare(
					b.outcome?.value.shortName || ""
				)
			)
			.at(0)?.outcome?.value?.shortName,
		skill: outcomesGroup.at(0)?.outcome?.row.id,
	}))

	return domain.rowsSet?.types?.map((skill) => {
		const filteredResults = listOfResults.filter(
			(result) => result.skill === skill.id
		)
		const totalDecayingAverage = filteredResults.reduce(
			(sum, result) => sum + (result.decayingAverage || 0),
			0
		)
		const averageDecayingAverage =
			totalDecayingAverage / filteredResults.length

		const sortedMasteryLevels = filteredResults
			.map((result) => result.masteryLevel)
			.filter((masteryLevel) => masteryLevel !== undefined)
			.sort((a, b) => (a || "").localeCompare(b || ""))

		const masteryLevel =
			sortedMasteryLevels[sortedMasteryLevels.length - 1] || null

		return {
			skill: skill.id,
			masteryLevel: masteryLevel,
			decayingAverage: averageDecayingAverage,
		} as DecayingAveragePerSkill
	})
}

/**
 * Calculate the averages for each task type divided in architecture layers.
 * @param submissions
 * @param domain
 * @constructor
 */
export const calculateAverageTaskOutcomes = (
	submissions: LearningDomainSubmission[],
	domain: LearningDomain
): DecayingAveragePerLayer[] => {
	const canvasDecaying = calculateDecayingAverageForAllOutcomes(
		submissions,
		domain
	)

	return domain.rowsSet?.types?.map((layer) => {
		const layerActivities = domain.columnsSet?.types!.map((activity) => {
			let totalScoreActivity = 0
			let totalScoreArchitectureActivity = 0
			let amountOfActivities = 0

			canvasDecaying.forEach((l) =>
				l.layerActivities
					.filter((la) => la.activity === activity.id)
					.forEach((la) => {
						totalScoreActivity += la.decayingAverage
						amountOfActivities++

						if (l.architectureLayer === layer.id) {
							totalScoreArchitectureActivity += la.decayingAverage
						}
					})
			)

			const decayingAverage =
				amountOfActivities > 0
					? (totalScoreArchitectureActivity / totalScoreActivity) *
					  (totalScoreActivity / amountOfActivities)
					: 0

			return {
				activity: activity.id,
				decayingAverage: decayingAverage,
			} as DecayingAveragePerActivity
		})

		return {
			architectureLayer: layer.id,
			layerActivities: layerActivities,
		} as DecayingAveragePerLayer
	})
}

/**
 * Calculate average of given tasks divided in architectural layers
 * @param submissions
 * @param domain
 * @constructor
 * @private
 */
export const calculateDecayingAverageForAllOutcomes = (
	submissions: LearningDomainSubmission[],
	domain: LearningDomain
): DecayingAveragePerLayer[] => {
	const results = getAllOutcomes(submissions)

	return domain.rowsSet.types.map((layer) => {
		const filteredResults = results.filter(
			(outcome) => outcome.outcome?.row.id === layer.id
		)
		const groupedResults = groupBy(filteredResults, (r) => r.outcome?.id!)

		const layerActivities = Object.entries(groupedResults).map(
			([outcomeId, activities]) => {
				const activityLayerId = activities[0]?.outcome?.column?.id!
				const decayingAverage =
					calculateDecayingAverageForOutcomeType(activities)

				return {
					outcome: outcomeId,
					activity: activityLayerId,
					decayingAverage: decayingAverage,
				}
			}
		)

		return {
			architectureLayer: layer.id!,
			layerActivities: layerActivities,
		}
	})
}

/**
 * Calculate decaying average described by Canvas: https://community.canvaslms.com/t5/Canvas-Basics-Guide/What-are-Outcomes/ta-p/75#decaying_average
 * @param results
 * @constructor
 * @private
 */
export const calculateDecayingAverageForOutcomeType = (
	results: LearningDomainOutcomeRecord[]
): number => {
	if (results.length === 0) {
		return 0.0
	}

	const recentResult = results[results.length - 1]
	let totalGradeScore = recentResult.grade || 0.0

	if (results.length > 1) {
		const sumOfGrades = results
			.slice(0, -1)
			.reduce((sum, r) => sum + (r.grade || 0.0), 0.0)
		const averageGrade = sumOfGrades / results.length
		totalGradeScore = averageGrade * 0.35 + (recentResult.grade || 0) * 0.65
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
	fn: (item: T) => number | string
): Record<string, T[]> => {
	return arr.reduce<Record<string, T[]>>((previous, current) => {
		const groupKey = fn(current)
		const group = previous[groupKey] || []
		group.push(current)
		return { ...previous, [groupKey]: group }
	}, {})
}
