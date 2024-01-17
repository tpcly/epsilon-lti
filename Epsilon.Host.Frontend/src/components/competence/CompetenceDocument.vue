<template>
	<div v-if="submissions" class="competence-document">
		<KpiTable
			:outcomes="allOutcomes"
			:submissions="filteredSubmissionsDateSelection" />
		<h2>Competence Profile</h2>
		<CompetenceProfile
			:submissions="filteredSubmissionsDateSelection"
			:domain="domains.find((l) => l.id == 'hbo-i-2018')" />
		<KpiMatrix
			v-if="outcomes.length > 0"
			:outcomes="outcomes"
			:submissions="filteredSubmissionsDateSelection" />
	</div>
</template>

<script lang="ts" setup>
import KpiMatrix from "~/components/competence/KpiMatrix.vue"
import KpiTable from "~/components/competence/KpiTable.vue"
import CompetenceProfile from "~/components/competence/CompetenceProfile.vue"
import type {
	LearningDomain,
	LearningDomainOutcome,
	LearningDomainSubmission,
} from "~/api.generated"

const componentProps = defineProps<{
	submissions: LearningDomainSubmission[]
	domains: LearningDomain[]
	outcomes: LearningDomainOutcome[]
	filterRange: {
		start: Date
		end: Date
		startCorrected: Date
	} | null
}>()

const filteredSubmissionsDateSelection = computed(() => {
	const unwrappedFilterRange = componentProps.filterRange

	if (!unwrappedFilterRange) {
		return componentProps.submissions
	}

	return componentProps.submissions.filter((submission) => {
		if (submission.criteria!.length > 0) {
			const submittedAt = new Date(submission.submittedAt!)

			return (
				submittedAt >= unwrappedFilterRange.start &&
				submittedAt <= unwrappedFilterRange.end
			)
		}
	})
})

const allOutcomes = computed<LearningDomainOutcome[]>(() =>
	componentProps.submissions.flatMap((submission) =>
		submission.results!.map((result) => result.outcome!)
	)
)
</script>

<style scoped>
.competence-document {
	display: grid;
	gap: 2rem 0;
}
</style>
