<template>
	<div v-if="store.state.filterdSubmissions" class="competence-document">
		<KpiTable></KpiTable>
		<div></div>
		<div></div>
		<CompetenceProfile
			:submissions="store.state.filterdSubmissions"></CompetenceProfile>
		<div></div>
		<div></div>
		<KpiMatrix></KpiMatrix>
	</div>
</template>

<script lang="ts" setup>
import store from "@/store"
import KpiMatrix from "@/components/Competence/KpiMatrix.vue"
import KpiTable from "@/components/Competence/KpiTable.vue"
import CompetenceProfile from "@/components/Competence/CompetenceProfile.vue"
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
