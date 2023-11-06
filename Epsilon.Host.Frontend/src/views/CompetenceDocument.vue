<template>
	<div v-if="store.state.filterdSubmissions" class="competence-document">
		<KpiTable></KpiTable>
		<CompetenceProfile
			:submissions="store.state.filterdSubmissions"></CompetenceProfile>
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
		display: flex;
		flex-wrap: wrap;
		align-items: center;
		justify-items: center;
		justify-content: space-between;
	}
}
</style>
