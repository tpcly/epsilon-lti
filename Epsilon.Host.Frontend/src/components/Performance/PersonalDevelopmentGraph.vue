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

onMounted(() => {
    if (store.state.personalDevelopment.rowsSet?.types != null) {
        store.state.personalDevelopment.rowsSet?.types.forEach(
            (s: LearningDomainType) => {
                chartOptions.xaxis.categories.push(s.shortName as never)
            }
        )
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
                fillColor: "#" + getMastery(d.masteryLevel)?.hexColor,
            }
        }),
    },
])

function getMastery(masteryId: number | null): LearningDomainType | undefined {
    if (store.state.domain.valuesSet?.types == null || masteryId == null) {
        return undefined
    }

    return store.state.domain.valuesSet?.types.find(
        (masteryLevel: LearningDomainType) =>
            (masteryLevel.shortName as unknown as number) == masteryId
    )
}
</script>
