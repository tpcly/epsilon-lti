<template>
    <ApexChart
        :options="chartOptions"
        :series="series"
        height="300"
        type="bar"
        width="200" />
</template>

<script lang="ts" setup>
import ApexChart from "vue3-apexcharts"
import { computed, onMounted } from "vue"
import { DecayingAverageLogic } from "@/DecayingAverageLogic"
import { useStore } from "vuex"
import { LearningDomainType } from "@/api"
const store = useStore()
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
            borderRadius: 0,
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

onMounted(() => {
    const rowTypes = store.state.personalDevelopment.rowsSet?.types
    if (rowTypes != null) {
        rowTypes.forEach((s: LearningDomainType) => {
            chartOptions.xaxis.categories.push(s.shortName as never)
        })
    }
})

const series = computed(() => [
    {
        name: "Score",
        data: DecayingAverageLogic.getAverageSkillOutcomeScores(
            store.state.filterdSubmissions,
            store.state.personalDevelopment
        )?.map((d) => {
            return {
                y: d.decayingAverage?.toFixed(3),
                x: store.state.personalDevelopment.rowsSet?.types?.find(
                    (s: LearningDomainType) => s.id == d.skill
                ).name,
                fillColor: "#" + getValue(d.masteryLevel)?.hexColor,
            }
        }),
    },
])

function getValue(valueId: number | null): LearningDomainType | undefined {
    if (store.state.domain.valuesSet?.types == null || valueId == null) {
        return undefined
    }

    return store.state.domain.valuesSet?.types.find(
        (masteryLevel: LearningDomainType) =>
            (masteryLevel.shortName as unknown as number) == valueId
    )
}
</script>