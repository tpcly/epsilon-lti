<template>
	<v-row class="mt-3">
		<v-col xs="12" md="8">
			<CompetenceProfile
				:submissions="filteredSubmissionsDateSelection"
				class="competence-profile"
				:is-loading="store.loadingSubmissions"
				:domain="service.getDomain(true)" />
		</v-col>
		<v-col xs="12" md="4">
			<LearningDomainValues :domain="service.getDomain(true)" />
		</v-col>
		<v-col cols="12">
			<h2>Kpi-Matrix</h2>
			<KpiMatrix
				v-if="store.outcomes.length > 0"
				:outcomes="store.outcomes"
				:submissions="filteredSubmissionsDateSelection" />
		</v-col>
	</v-row>
</template>

<script lang="ts" setup>
import KpiMatrix from "~/components/competence/KpiMatrix.vue"
import CompetenceProfile from "~/components/competence/CompetenceProfile.vue"
import { useEpsilonStore } from "~/stores/use-store"
const store = useEpsilonStore()
const service = useServices()

const filteredSubmissionsDateSelection = computed(() => {
	const unwrappedFilterRange = store.selectedTermRange

	if (!unwrappedFilterRange) {
		return store.submissions
	}

	return store.submissions.filter((submission) => {
		if (submission.criteria!.length > 0) {
			const submittedAt = new Date(submission.submittedAt!)

			return (
				submittedAt >= unwrappedFilterRange.start &&
				submittedAt <= unwrappedFilterRange.end
			)
		}
	})
})
</script>

<style scoped>
.competence-document {
	display: grid;
	gap: 2rem 0;
}
</style>
