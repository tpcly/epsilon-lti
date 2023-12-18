import type {
	LearningDomainCriteria,
	LearningDomainOutcome,
	LearningDomainSubmission,
} from "~/api.generated"

export class Generator {
	static date(): string {
		const date = new Date()
		return date.getFullYear() + "-" + date.getMonth() + "-" + date.getDate()
	}

	static generateSubmissions(
		outcomes: LearningDomainOutcome[]
	): LearningDomainSubmission[] {
		const learningDomainSubmissions: LearningDomainSubmission[] = []
		for (let i = 0; i < 10; i++) {
			const learningDomainSubmission: LearningDomainSubmission = {
				assignment: `Submission ${i}`,
				assignmentUrl: `https://www.deltafhict.nl`,
				submittedAt: this.date(),
				results: [],
				criteria: [],
			}

			for (let x = 0; x < 6; x++) {
				const outcome = outcomes.at(
					Math.floor(Math.random() * Math.floor(outcomes.length))
				)
				const criteria: LearningDomainCriteria = {
					id: outcome?.id,
					masteryPoints: 3,
				}
				learningDomainSubmission.criteria?.push(criteria)
				learningDomainSubmission.results?.push({
					outcome: outcome,
					grade: Math.floor(Math.random() * (6 - 3 + 1) + 3),
				})
			}

			learningDomainSubmissions.push(learningDomainSubmission)
		}

		return learningDomainSubmissions
	}
}
