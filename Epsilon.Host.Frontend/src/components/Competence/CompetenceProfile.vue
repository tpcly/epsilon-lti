<template>
    <table>
        <thead>
            <tr>
                <td></td>
                <th
                    v-for="col of store.state.domain.columnsSet?.types"
                    :key="col.id">
                    {{ col.name }}
                </th>
            </tr>
            <tr v-for="row of store.state.domain.rowsSet?.types" :key="row.id">
                <th>
                    {{ row.name }}
                </th>
                <CompetenceProfileCell
                    v-for="col of store.state.domain.columnsSet?.types"
                    :key="col.id"
                    :count="
                        props.submissions
                            .map(
                                (s) =>
                                    s.results?.filter(
                                        (r) =>
                                            r.outcome?.row?.id == row.id &&
                                            r.outcome?.column?.id == col.id &&
                                            r.grade >= 3
                                    ).length
                            )
                            .reduce((sum, current) => {
                                if (typeof current === 'number') {
                                    return sum + current
                                }
                                return sum
                            }, 0)
                    "
                    :result="getFilterd(row, col, allOutcomes)">
                </CompetenceProfileCell>
            </tr>
        </thead>
    </table>
</template>

<script setup lang="ts">
import { useStore } from "vuex"

const store = useStore()
import { computed } from "vue"
import {
    LearningDomainOutcome,
    LearningDomainSubmission,
    LearningDomainType,
} from "@/api"
import CompetenceProfileCell from "@/components/Competence/CompetenceProfileCell.vue"

const props = defineProps<{
    submissions: LearningDomainSubmission[]
}>()

const allOutcomes = computed(() =>
    props.submissions
        .flatMap((sub) => sub.criteria?.map((cr) => cr.id))
        .filter((value, index, self) => self.indexOf(value) === index)
) as unknown as number[]

function getFilterd(
    row: LearningDomainType,
    col: LearningDomainType,
    outcomes: number[]
): LearningDomainOutcome | null {
    return store.state.outcomes
        .filter(
            (o: LearningDomainOutcome) =>
                o?.row?.id === row.id &&
                o?.column?.id === col.id &&
                outcomes.includes(o.id as number)
        )
        .reduce((acc: LearningDomainOutcome, value: LearningDomainOutcome) => {
            return (acc?.value?.shortName as unknown as number) >
                (value?.value?.shortName as unknown as number)
                ? acc
                : value
        }, null)
}
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
