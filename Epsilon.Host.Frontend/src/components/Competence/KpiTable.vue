<template>
    <h2>KPI-Table</h2>
    <table>
        <tr v-for="outcome of allOutcomes.sort()" :key="outcome">
            <th>
                {{ store.state.outcomes.find((o) => o.id === outcome).name }}
            </th>
            <td>
                <div
                    v-for="submission of store.state.filterdSubmissions.filter(
                        (s) =>
                            s.results.filter((r) => r.outcome.id === outcome)
                                .length > 0
                    )"
                    :key="submission.submittedAt">
                    <a :href="submission.assignmentUrl" target="_blank">{{
                        submission.assignment
                    }}</a>
                </div>
            </td>
            <td>
                <div
                    v-for="submission of store.state.filterdSubmissions.filter(
                        (s) =>
                            s.results.filter((r) => r.outcome.id === outcome)
                                .length > 0
                    )"
                    :key="submission.submittedAt">
                    {{
                        submission.results.find((r) => r.outcome.id === outcome)
                            ?.grade
                    }}
                </div>
            </td>
        </tr>
    </table>
</template>
<script setup lang="ts">
import { useStore } from "vuex"
import { computed } from "vue"

const store = useStore()

const allOutcomes = computed(() =>
    store.state.filterdSubmissions
        .flatMap((sub) => sub.results?.map((r) => r.outcome?.id))
        .filter((value, index, self) => self.indexOf(value) === index)
) as unknown as number[]
</script>
<style scoped lang="scss">
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
</style>
