<template>
	<table class="competence-profile-legend">
		<tr
			v-for="level of store.state.domain.valuesSet.types.sort((a,b) => a.shortName - b.shortName) as LearningDomainType[]"
			:key="level"
			class="competence-profile-legend-row">
			<td
				class="value-color"
				:style="{ backgroundColor: '#' + level.hexColor }" />
			<th class="value-text">{{ level.name }}</th>
		</tr>
		<tr class="masteryLegend mastery-line-legend">
			<td
				class="masteryLegend-icon"
				:style="{ backgroundColor: masteryLineColor }" />
			<th class="masteryLegend-text">Mastery Line</th>
		</tr>
	</table>
</template>

<script lang="ts" setup>
import store from "@/store"
import { LearningDomainType } from "@/api.generated"
const masteryLineColor = "red"
</script>

<style>
.competence-profile-legend {
	display: flex;
	flex-direction: column;
	width: fit-content;
	height: fit-content;
	padding: 2rem;
}

.value-text {
	font-weight: 400;
}

.value-color {
	width: 3rem;
}

.competence-profile-legend-row {
	display: grid;
	grid-template-columns: repeat(2, 5rem);
	padding: 0.5rem;
}

.masteryLegend {
	display: flex;
	align-items: center;
	margin-top: 10px;
}

.masteryLegend-text {
	margin-left: 51px;
	font-weight: 400;
}

.masteryLegend-icon {
	width: 45px;
	height: 2px;
	margin-left: 8px;
	background-color: red;
}
</style>
