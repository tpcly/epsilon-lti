<template>
	<ClientOnly>
		<ApexChart
			:options="chartOptions"
			:series="series"
			height="300"
			type="bar"
			class="competence-graph"
			style="max-width: 100%; width: 450px" />
	</ClientOnly>
</template>

<script lang="ts" setup>
import ApexChart from "vue3-apexcharts"
import {
	type DecayingAveragePerLayer,
	calculateAverageTaskOutcomes,
} from "~/utils/decaying-average"
import {
	type LearningDomain,
	type LearningDomainSubmission,
} from "~/api.generated"

const componentProps = defineProps<{
	domain: LearningDomain
	submissions: LearningDomainSubmission[]
	isLoading: boolean
}>()

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
			show: false,
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
			borderRadius: 0,
			dataLabels: {},
		},
	},
	xaxis: {
		type: "string",
		categories: [],
		labels: {
			show: true,
			rotate: 0,
			style: {
				fontSize: "12.5px",
			},
		},
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
		y: {
			formatter: function (value: number): string {
				return ((value * 100) / 5).toFixed(0) + "%"
			},
		},
	},
}

onMounted(() => {
	const columnTypes = componentProps.domain?.columnsSet?.types
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
	return calculateAverageTaskOutcomes(
		componentProps.submissions,
		componentProps.domain
	)?.map((layer: DecayingAveragePerLayer) => {
		const row = componentProps.domain?.rowsSet?.types.find(
			(l) => l.id === layer.architectureLayer
		)

		return {
			name: row?.name,
			color: "#" + row?.hexColor + (componentProps.isLoading ? "80" : ""),
			data: layer.layerActivities.map((column) =>
				column.decayingAverage.toFixed(3)
			),
		}
	})
})
</script>
