<template>
	<div class="performance-dashboard">
		<CompetenceProfile
			:submissions="submissions"
			:domain="domains.find((l) => l.id == 'hbo-i-2018')" />
		<LearningDomainValues
			:domain="domains.find((l) => l.id == 'hbo-i-2018')" />
		<div />
		<CompetenceGraph
			:domain="domains.find((l) => l.id == 'hbo-i-2018')"
			:submissions="submissions" />
		<PersonalDevelopmentGraph
			:domain="domains.find((l) => l.id == 'pd-2020-bsc')"
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
	domains: LearningDomain[]
}>()
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
