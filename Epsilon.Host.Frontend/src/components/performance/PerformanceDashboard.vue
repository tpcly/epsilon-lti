<template>
	<div class="performance-dashboard">
		<v-row>
			<v-col xs="12" md="8">
				<CompetenceProfile
					:submissions="store.filteredSubmissions"
					:is-loading="store.loadingSubmissions"
					class="competence-profile"
					:domain="
						store.domains.find(
							(l) =>
								store.usedDomains.includes(l.id!) &&
								l.columnsSet != undefined
						)!
					" />
			</v-col>
			<v-col xs="12" md="4">
				<LearningDomainValues
					:domain="
						store.domains.find(
							(l) =>
								store.usedDomains.includes(l.id!) &&
								l.columnsSet != undefined
						)!
					" />
			</v-col>
			<v-col xs="12" md="8">
				<CompetenceGraph
					v-if="store.domains.length > 1 && store.filteredSubmissions"
					class="competence-graph"
					:is-loading="store.loadingSubmissions"
					:domain="
						store.domains.find(
							(l) =>
								store.usedDomains.includes(l.id!) &&
								l.columnsSet != undefined
						)!
					"
					:submissions="store.filteredSubmissions" />
			</v-col>
			<v-col xs="12" md="4">
				<PersonalDevelopmentGraph
					v-if="store.domains.length > 1 && store.filteredSubmissions"
					:is-loading="store.loadingSubmissions"
					:domain="
						store.domains.find(
							(l) =>
								store.usedDomains.includes(l.id!) &&
								l.columnsSet == undefined
						)!
					"
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
</script>

<style>
.performance-dashboard .competence-graph {
	float: right;
}
.performance-dashboard .competence-profile {
	float: right;
}
</style>
