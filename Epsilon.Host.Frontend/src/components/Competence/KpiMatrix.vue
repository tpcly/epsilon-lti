<template>
    <h2>KPI-Matrix</h2>
    <table>
        <thead>
            <tr>
                <th></th>
                <th
                    v-for="submission of store.state.filterdSubmissions"
                    :key="submission.assignment">
                    {{ submission.assignment }}
                </th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="outcome of allOutcomes.sort()" :key="outcome">
                <th>
                    {{ store.state.outcomes.find((o) => o.id == outcome).name }}
                </th>
                <KpiMatrixCell
                    v-for="submission of store.state.filterdSubmissions"
                    :result="
                        submission.results.find((r) => r.outcome.id == outcome)
                    "
                    :criteria="
                        submission.criteria.find((c) => c.id === outcome)
                    "></KpiMatrixCell>
            </tr>
        </tbody>
    </table>
</template>
<script lang="ts" setup>
import { computed } from "vue"
import { useStore } from "vuex"
import KpiMatrixCell from "@/components/Competence/KpiMatrixCell.vue"
const store = useStore()

const allOutcomes = computed(
    () =>
        store.state.filterdSubmissions
            .flatMap((sub) => sub.criteria?.map((cr) => cr.id))
            .filter((value, index, self) => self.indexOf(value) === index)
    // .map((uniqueOutcome) => props.domain.)
)

console.log(allOutcomes)

// const assignments = computed(() =>
//     DecayingAverageLogic.groupBy(
//         props.results,
//         (r) => r.assignment as unknown as string
//     )
// )
//
// const resultsByOutcome = computed(() =>
//     DecayingAverageLogic.groupBy(
//         props.results,
//         (r) => r.outcome?.id as unknown as string
//     )
// )

// function getColor(status?: OutcomeGradeStatus): string {
//     switch (status) {
//         case OutcomeGradeStatus.Value0:
//             return "44F656"
//         case OutcomeGradeStatus.Value1:
//             return "FA1818"
//         case OutcomeGradeStatus.Value2:
//             return "FAFF00"
//         case OutcomeGradeStatus.Value3:
//             return "9F2B68"
//         default:
//             return ""
//     }
// }
</script>
<style lang="scss" scoped>
tr {
    border: 3px lightgray solid;
}

tr td,
tr th {
    padding: 10px;
}

td div {
    border-bottom: 1px lightgray solid;
    width: 100%;
}
thead th {
    writing-mode: vertical-lr;
}
</style>
