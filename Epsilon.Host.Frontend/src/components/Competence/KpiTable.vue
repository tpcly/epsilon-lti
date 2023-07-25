<template>
    <h2>KPI-Table</h2>
    <table>
        <tr
            v-for="outcomeResults of DecayingAverageLogic.groupBy(
                results,
                (r) => r.outcome?.id
            )"
            :key="outcomeResults.at(0).outcome.id">
            <th>{{ outcomeResults.at(0).outcome.name }}</th>
            <td>
                <div
                    v-for="assignment of outcomeResults"
                    :key="assignment.assignment + assignment.outcome?.id">
                    <a :href="assignment.assignmentUrl" target="_blank">{{
                        assignment.assignment
                    }}</a>
                    <br />
                </div>
            </td>
            <td>
                <div
                    v-for="grade of outcomeResults"
                    :key="grade.assignment + grade.outcome?.id">
                    {{ grade.grade }}
                    <br />
                </div>
            </td>
        </tr>
    </table>
</template>
<script setup lang="ts">
import { defineProps } from "vue"
import { LearningDomain, LearningDomainOutcomeResult } from "@/api"
import { DecayingAverageLogic } from "@/logic/DecayingAverage"

const props = defineProps<{
    domain: LearningDomain
    results: LearningDomainOutcomeResult[]
}>()
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
