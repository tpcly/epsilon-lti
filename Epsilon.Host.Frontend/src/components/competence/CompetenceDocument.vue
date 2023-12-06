<template>
	<div v-if="submissions" class="competence-document">
		<KpiTable :submissions="submissions" :outcomes="allOutcomes" />
		<CompetenceProfile
			:submissions="submissions"
			:domain="domains.find((l) => l.id == 'hbo-i-2018')" />
		<KpiMatrix :submissions="submissions" />
	</div>
</template>

<script lang="ts" setup>
import KpiMatrix from "~/components/competence/KpiMatrix.vue"
import KpiTable from "~/components/competence/KpiTable.vue"
import CompetenceProfile from "~/components/competence/CompetenceProfile.vue"
import type {
	LearningDomain,
	LearningDomainSubmission,
	LearningDomainOutcome,
} from "~/api.generated"

const props = defineProps<{
	submissions: LearningDomainSubmission[]
	domains: LearningDomain[]
}>()

const allOutcomes = computed<LearningDomainOutcome[]>(() =>
	props.submissions.flatMap((submission) =>
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
