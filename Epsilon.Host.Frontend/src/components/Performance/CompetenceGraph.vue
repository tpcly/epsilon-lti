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
import { onMounted, watchEffect } from "vue"
import { LearningDomain, LearningDomainSubmission } from "@/api"
let series: Array<{
    name: string
    color: string
    data: Array<string | number> | undefined
}> = []
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

function loadChartData(): void {
    series = []
    chartOptions.xaxis.categories = []
    if (store.state.domain?.columnsSet?.types != null) {
        store.state.domain?.columnsSet?.types.forEach((s) => {
            chartOptions.xaxis.categories.push(s.name as never)
        })
    }

    DecayingAverageLogic.getAverageTaskOutcomeScores(
        store.state.submissions as LearningDomainSubmission[],
        store.state.domain as LearningDomain
    ).map((layer: DecayingAveragePerLayer) => {
        const ar = store.state.domain?.rowsSet?.types?.find(
            (l) => l.id === layer.architectureLayer
        )
        series.push({
            name: ar?.name as string,
            color: "#" + ar?.hexColor,
            data: layer.layerActivities.map((ac) =>
                ac.decayingAverage.toFixed(3)
            ),
        })
    })
}

onMounted(() => {
    loadChartData()
})
watchEffect(() => loadChartData())
</script>

<style scoped>
.competence-graph {
    margin-left: auto;
}
</style>
