<template>
	<div class="performance-dashboard">
		<CompetenceProfile
			v-if="hboIDomain"
			:submissions="submissions"
			:domain="hboIDomain" />
		<LearningDomainValues v-if="hboIDomain" :domain="hboIDomain" />
		<div />
		<CompetenceGraph
			v-if="hboIDomain"
			:domain="hboIDomain"
			:submissions="submissions" />
		<PersonalDevelopmentGraph
			v-if="personalDevelopmentDomain"
			:domain="personalDevelopmentDomain"
			:submissions="submissions" />
	</div>
</template>

<script lang="ts" setup>
import CompetenceProfile from "~/components/competence/CompetenceProfile.vue"
import CompetenceGraph from "~/components/performance/CompetenceGraph.vue"
import PersonalDevelopmentGraph from "~/components/performance/PersonalDevelopmentGraph.vue"
import type { LearningDomain, LearningDomainSubmission } from "~/api.generated"

defineProps<{
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
</script>

<style scoped>
.performance-dashboard {
	grid-template-columns: 1fr;
}

@media screen and (min-width: 580px) {
	.performance-dashboard {
		display: grid;
		grid-template-columns: 1fr 5fr 1fr;
		gap: 2rem 0;
		align-items: center;
		justify-items: center;
	}
}
</style>
