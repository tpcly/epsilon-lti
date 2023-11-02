<template>
	<div v-if="store.state.filterdSubmissions" class="competence-document">
		<KpiTable></KpiTable>
		<KpiMatrix></KpiMatrix>
		<CompetenceProfile
			:submissions="store.state.filterdSubmissions"></CompetenceProfile>
	</div>
</template>

<script lang="ts" setup>
import store from "@/store"
import KpiMatrix from "@/components/Competence/KpiMatrix.vue"
import KpiTable from "@/components/Competence/KpiTable.vue"
import CompetenceProfile from "@/components/Competence/CompetenceProfile.vue"
import { LearningDomainSubmission } from "@/api.generated"

const submissions: LearningDomainSubmission[] = [];
</script>

<style scoped>
.competence-document {
	grid-template-columns: 1fr;
}

@media screen and (min-width: 580px) {
	.competence-document {
		max-width: 100%;
		grid-template-columns: 1fr 5fr 1fr;
		gap: 2rem 0;
		align-items: center;
		justify-items: center;
	}
}
</style>
