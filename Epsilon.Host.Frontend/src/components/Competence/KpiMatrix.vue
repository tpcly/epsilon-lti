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
            <tr
                v-for="outcome of allOutcomes.sort() as string[]"
                :key="outcome">
                <th>
                    {{ store.state.outcomes.find((o) => o.id == outcome).name }}
                </th>
                <KpiMatrixCell
                    v-for="submission of store.state.filterdSubmissions"
                    :key="submission.assignmentUrl"
                    :result="
                        submission.results?.find(
                            (r) => r?.outcome?.id == outcome
                        )
                    "
                    :criteria="
                        submission.criteria?.find((c) => c?.id == outcome)
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
            .flatMap((sub) => sub.criteria?.map((cr) => cr.id as string))
            .filter((value, index, self) => self.indexOf(value) === index)
    // .map((uniqueOutcome) => props.domain.)
)
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
