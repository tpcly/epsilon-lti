<template>
	<div v-if="submissions" class="competence-document">
		<KpiTable :submissions="submissions" :outcomes="allOutcomes" />
		<CompetenceProfile
			v-if="hboIDomain"
			:submissions="submissions"
			:domain="hboIDomain" />
		<KpiMatrix
			v-if="hboIDomain"
			:submissions="submissions"
			:domain="hboIDomain" />
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
}>()

const api = useApi()

const hboIDomain = ref<LearningDomain | null>(null)
const personalDevelopmentDomain = ref<LearningDomain | null>(null)

api.learning.domainDetail("hbo-i-2018").then((hboIData) => {
	hboIDomain.value = hboIData.data
})
api.learning.domainDetail("pd-2020-bsc").then((personalDevelopmentData) => {
	personalDevelopmentDomain.value = personalDevelopmentData.data
})

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
