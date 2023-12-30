<template>
	<div class="performance-dashboard">
		<v-row>
			<v-col xs="12" md="8">
				<CompetenceProfile
					:submissions="submissions"
					:is-loading="isLoading"
					class="competence-profile"
					:domain="domains.find((l) => l.id == 'hbo-i-2018')" />
			</v-col>
			<v-col xs="12" md="4">
				<LearningDomainValues
					:domain="domains.find((l) => l.id == 'hbo-i-2018')" />
			</v-col>
			<v-col xs="12" md="8">
				<CompetenceGraph
					v-if="domains.length > 1 && submissions"
					class="competence-graph"
					:is-loading="isLoading"
					:domain="domains.find((l) => l.id == 'hbo-i-2018')"
					:submissions="submissions" />
			</v-col>
			<v-col xs="12" md="4">
				<PersonalDevelopmentGraph
					v-if="domains.length > 1 && submissions"
					:is-loading="isLoading"
					:domain="domains.find((l) => l.id == 'pd-2020-bsc')"
					:submissions="submissions" />
			</v-col>
		</v-row>
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
	isLoading: boolean
}>()
</script>

<style>
.performance-dashboard .competence-graph {
	float: right;
}
.performance-dashboard .competence-profile {
	float: right;
}
</style>
