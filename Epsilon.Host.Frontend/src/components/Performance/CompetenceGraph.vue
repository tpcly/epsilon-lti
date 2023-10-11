<template>
	<ApexChart
		:options="chartOptions"
		:series="series"
		height="300"
		type="bar"
		class="competence-graph"
		width="490" />
</template>

<script lang="ts" setup>
import ApexChart from "vue3-apexcharts"
import {
	DecayingAverageLogic,
	DecayingAveragePerLayer,
} from "@/DecayingAverageLogic"
import store from "@/store"
import { computed, onMounted } from "vue"
import { LearningDomain, LearningDomainSubmission, LearningDomainType } from "@/api.generated"

const chartOptions = {
	annotations: {
		yaxis: [
			{
				y: 3,
				borderColor: "red",
				strokeDashArray: 0,
				label: {
					borderColor: "red",
					style: {
						color: "#fff",
						background: "red",
					},
					text: "Mastery",
				},
			},
		],
	},
	colors: [],
	chart: {
		type: "bar",
		height: 300,
		stacked: true,
		toolbar: {
			show: true,
		},
		zoom: {
			enabled: false,
		},
	},
	dataLabels: {
		enabled: false,
	},
	plotOptions: {
		bar: {
			horizontal: false,
			borderRadius: 4,
			dataLabels: {},
		},
	},
	xaxis: {
		type: "string",
		categories: [],
	},
	yaxis: {
		show: false,
	},
	legend: {
		show: false,
	},
	fill: {
		opacity: 1,
	},
	tooltip: {
		enabled: true,
	},
}

onMounted(() => {
	const columnTypes = store.state.domain?.columnsSet?.types.sort((a: LearningDomainType, b: LearningDomainType) => (a.order || 0) - (b.order || 0))
	if (columnTypes != undefined) {
		columnTypes.forEach((s) => {
			chartOptions.xaxis.categories.push(s.name as never)
		})
	}
})

/**
 * Function that creates the "series" dataset to present in the graph.
 * For every rowSet in the domain and the underlying column the decaying average has been calculated.
 * Then this data is formed in the format that ApexCharts can present the data.
 * an array with objects for each row: The name of the row, Corresponding color and an array with scores.
 */
const series = computed(() => {
	return DecayingAverageLogic.getAverageTaskOutcomeScores(
		store.state.filterdSubmissions as LearningDomainSubmission[],
		store.state.domain as LearningDomain
	).map((layer: DecayingAveragePerLayer) => {
		const row = store.state.domain?.rowsSet?.types?.sort((a: LearningDomainType, b: LearningDomainType) => (a.order || 0) - (b.order || 0)).find(
			(l) => l.id === layer.architectureLayer
		)
		return {
			name: row?.name as string,
			color: "#" + row?.hexColor,
			data: layer.layerActivities.map((column) =>
				column.decayingAverage.toFixed(3)
			),
		}
	})
})
</script>

<style scoped>
.competence-graph {
	margin-left: auto;
}
</style>
