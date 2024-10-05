<template>
	<div class="performance-dashboard">
		<v-row>
			<v-col xs="12" md="8">
				<CompetenceProfile
					:submissions="store.filteredSubmissions"
					:is-loading="store.loadingSubmissions"
					class="competence-profile"
					:title="''"
					:domain="service.getDomain(true)" />
			</v-col>
			<v-col xs="12" md="4">
				<LearningDomainValues :domain="service.getDomain(false)" />
			</v-col>
			<v-col xs="12" md="8">
				<CompetenceGraph
					v-if="store.domains.length > 1 && store.filteredSubmissions"
					class="competence-graph"
					:is-loading="store.loadingSubmissions"
					:domain="service.getDomain(true)"
					:submissions="store.filteredSubmissions" />
			</v-col>
			<v-col xs="12" md="4">
				<PersonalDevelopmentGraph
					v-if="store.domains.length > 1 && store.filteredSubmissions"
					:is-loading="store.loadingSubmissions"
					:domain="service.getDomain(false)"
					:submissions="store.filteredSubmissions" />
			</v-col>
		</v-row>
	</div>
</template>

<script lang="ts" setup>
import CompetenceProfile from "~/components/competence/CompetenceProfile.vue"
import CompetenceGraph from "~/components/performance/CompetenceGraph.vue"
import PersonalDevelopmentGraph from "~/components/performance/PersonalDevelopmentGraph.vue"
import { useEpsilonStore } from "~/stores/use-store"

const store = useEpsilonStore()
const service = useServices()
</script>

<style>
.performance-dashboard .competence-graph {
	float: right;
}
.performance-dashboard .competence-profile {
	float: right;
}
</style>
