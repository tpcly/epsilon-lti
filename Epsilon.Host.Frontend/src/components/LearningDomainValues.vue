<template>
	<table v-if="domain" class="learning-domain-values">
		<tr
			v-for="type of types"
			:key="type"
			class="learning-domain-values-row">
			<td
				class="learning-domain-values-value-color"
				:style="{ backgroundColor: '#' + type.hexColor }" />
			<th class="learning-domain-values-value-text">
				{{ type.name }}
			</th>
		</tr>
		<tr class="masteryLegend mastery-line-legend">
			<td
				class="masteryLegend-icon"
				:style="{ backgroundColor: masteryLineColor }" />
			<th class="masteryLegend-text">Mastery</th>
		</tr>
	</table>
	<table v-else class="learning-domain-values">
		<tr v-for="type of 4" :key="type" class="learning-domain-values-row">
			<td
				class="learning-domain-values-value-color"
				:style="{ backgroundColor: '#11284C' }" />
			<th class="learning-domain-values-value-text">{{ type }}</th>
		</tr>
		<tr class="masteryLegend mastery-line-legend">
			<td
				class="masteryLegend-icon"
				:style="{ backgroundColor: masteryLineColor }" />
			<th class="masteryLegend-text">Mastery</th>
		</tr>
	</table>
</template>

<script lang="ts" setup>
import { type LearningDomain } from "~/api.generated"
const masteryLineColor = "red"
const props = defineProps<{
	domain: LearningDomain
}>()

// TODO: Centralize this ugly sorting compare function, maybe possible with lodash?
const types = computed(() => {
	const types = props.domain?.valuesSet.types
	return types?.sort((a, b) => (a.order ?? 0) - (b.order ?? 0))
})
</script>

<style lang="scss">
.learning-domain-values {
	display: flex;
	flex-direction: column;
	width: fit-content;
	height: fit-content;
	padding: 2rem;

	&-row {
		display: grid;
		grid-template-columns: repeat(2, 5rem);
		padding: 0.5rem;
	}

	&-value {
		&-text {
			font-weight: 400;
		}

		&-color {
			width: 3rem;
		}
	}
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
