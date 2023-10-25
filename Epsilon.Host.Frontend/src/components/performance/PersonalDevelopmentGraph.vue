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
import {
    type LearningDomain,
    type LearningDomainSubmission,
    type LearningDomainType,
} from "~/api.generated"

import { getAverageSkillOutcomeScores } from "~/decaying-average"

const props = defineProps<{
    domain: LearningDomain
    submissions: LearningDomainSubmission[]
}>()

onMounted(() => {
    const rowTypes = props.domain.rowsSet.types

    rowTypes.forEach((s: LearningDomainType) => {
        chartOptions.xaxis.categories.push(s.shortName as never)
    })
})

const series = computed(() => [
    {
        name: "Score",
        data: getAverageSkillOutcomeScores(
            props.submissions,
            props.domain,
        )?.map((d) => {
            return {
                y: d.decayingAverage?.toFixed(3),
                x: props.domain.rowsSet.types.find(
                    (s: LearningDomainType) => s.id == d.skill,
                ).name,
                fillColor: "#" + getValue(d.masteryLevel)?.hexColor,
            }
        }),
    },
])

const getValue = (valueId: number): LearningDomainType =>
    props.domain.valuesSet.types.find(
        (masteryLevel: LearningDomainType) => (masteryLevel.shortName) == valueId,
    )

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
        categories: [],
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
