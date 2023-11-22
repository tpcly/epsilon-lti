<template>
	<div v-if="submissions" class="competence-document">
		<KpiTable :submissions="submissions" :outcomes="allOutcomes" />
		<div></div>
		<div></div>
		<CompetenceProfile
            v-if="hboIDomain"
			:submissions="submissions"
            :domain="hboIDomain" />
		<div></div>
		<div></div>
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

onMounted(async () => {
	hboIDomain.value = (await api.learning.domainDetail("hbo-i-2018")).data
	personalDevelopmentDomain.value = (
		await api.learning.domainDetail("pd-2020-bsc")
	).data
})

const allOutcomes = computed<LearningDomainOutcome[]>(() =>
	props.submissions.flatMap((submission) =>
		submission.results!.map((result) => result.outcome!)
	)
)
</script>

<style scoped>
.competence-document {
    grid-template-columns: 1fr;
}
@media screen and (min-width: 580px) {
    .competence-document {
        display: grid;
        grid-template-columns: 1fr 5fr 1fr;
        gap: 2rem 0;
        justify-content: space-between;
    }
}
</style>