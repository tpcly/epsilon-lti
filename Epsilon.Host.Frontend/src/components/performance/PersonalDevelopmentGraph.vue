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

const props = defineProps<{
	domain: LearningDomain
	submissions: LearningDomainSubmission[]
}>()

onMounted(() => {
	const rowTypes = props.domain.rowsSet.types

	rowTypes.forEach((s: LearningDomainType) => {
		chartOptions.xaxis.categories.push(s.shortName)
	})
})

const series = computed(() => [
	{
		name: "Score",
		data: calculateAverageSkillOutcomes(
			props.submissions,
			props.domain
		)?.map((d) => {
			return {
				y: d.decayingAverage?.toFixed(3),
				x: props.domain.rowsSet.types.find(
					(s: LearningDomainType) => s.id == d.skill
				)!.name,
				fillColor:
					"#" + getValueType(d.masteryLevel!.toString())?.hexColor,
			}
		}),
	},
])

const getValueType = (id: string): LearningDomainType =>
	props.domain.valuesSet.types.find(
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
					text: "Mastery",
				},
			},
		],
	},
	colors: ["#FFFFFF"],
	chart: {
		type: "bar",
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
		categories: [] as string[],
	},
	yaxis: {
		show: false,
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
