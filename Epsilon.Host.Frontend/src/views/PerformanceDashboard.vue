<template>
	<div v-if="store.state.filterdSubmissions" class="performance-dashboard">
		<CompetenceProfile
			:submissions="store.state.filterdSubmissions"></CompetenceProfile>
		<CompetenceProfileLegend></CompetenceProfileLegend>
		<div></div>
		<CompetenceGraph></CompetenceGraph>
		<PersonalDevelopmentGraph></PersonalDevelopmentGraph>
	</div>
</template>

<script lang="ts" setup>
import CompetenceGraph from "@/components/Performance/CompetenceGraph.vue"
import store from "@/store"
import PersonalDevelopmentGraph from "@/components/Performance/PersonalDevelopmentGraph.vue"
import CompetenceProfileLegend from "@/components/CompetenceProfileLegend.vue"
import CompetenceProfile from "@/components/Competence/CompetenceProfile.vue"
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
