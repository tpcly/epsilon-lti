<template>
	<ClientOnly>
		<ApexChart
			:options="chartOptions"
			:series="series"
			height="300"
			type="bar"
			width="200" />
	</ClientOnly>
</template>

<script lang="ts" setup>
import ApexChart from "vue3-apexcharts"
import { calculateAverageSkillOutcomes } from "~/utils/decaying-average"
import {
	type LearningDomain,
	type LearningDomainSubmission,
	type LearningDomainType,
} from "~/api.generated"

const componentProps = defineProps<{
	domain: LearningDomain
	submissions: LearningDomainSubmission[]
	isLoading: boolean
}>()

watch(componentProps.domain, () => {
	const rowTypes = componentProps.domain?.rowsSet?.types

	rowTypes.forEach((s: LearningDomainType) => {
		chartOptions.xaxis.categories.push(s.shortName)
	})
})

const series = computed(() => [
	{
		name: "Score",
		data: calculateAverageSkillOutcomes(
			componentProps.submissions,
			componentProps.domain
		)?.map((d) => {
			return {
				y: d.decayingAverage?.toFixed(3),
				x: componentProps.domain.rowsSet.types.find(
					(s: LearningDomainType) => s.id == d.skill
				)!.shortName,
				fillColor:
					"#" +
					getValueType(d.masteryLevel?.toString())?.hexColor +
					(componentProps.isLoading ? "80" : ""),
			}
		}),
	},
])

const getValueType = (id: string | undefined): LearningDomainType =>
	componentProps.domain.valuesSet.types.find(
		(masteryLevel: LearningDomainType) => masteryLevel.shortName == id
	)!

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
	colors: ["#FFFFFF"],
	chart: {
		type: "bar",
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
		categories: [] as string[],
	},
	yaxis: {
		show: false,
		min: 0,
		max: 5,
	},
	legend: {
		position: "bottom",
	},
	fill: {
		opacity: 1,
	},
	tooltip: {
		enabled: true,
	},
}
</script>
