<template>
    <table class="competence-profile">
        <thead>
        <tr>
            <td />
            <th
                v-for="col of domain.columnsSet?.types"
                :key="col.id"
                class="competence-profile-header competence-profile-header-col">
                {{ col.name }}
            </th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="row of domain.rowsSet.types" :key="row.id">
            <th
                class="competence-profile-header competence-profile-header-row">
                <div
                    class="competence-profile-header-color"
                    :style="{backgroundColor: '#' + row.hexColor}" />
                {{ row.name }}
            </th>
            <CompetenceProfileCell
                class="competence-profile-cell"
                v-for="col of domain.columnsSet?.types"
                :outcomes="outcomes(row, col)">
            </CompetenceProfileCell>
        </tr>
        </tbody>
    </table>
</template>

<script setup lang="ts">
import { computed } from "vue"
import {
    type LearningDomain,
    type LearningDomainOutcome,
    type LearningDomainSubmission,
    type LearningDomainType,
} from "~/api.generated"
import CompetenceProfileCell from "~/components/competence/CompetenceProfileCell.vue"

const props = defineProps<{
    domain: LearningDomain
    submissions: LearningDomainSubmission[]
}>()

const allOutcomes = computed<LearningDomainOutcome[]>(() =>
    props.submissions.flatMap(submission =>
        submission.results!.map(result => result.outcome!),
    ),
)

const outcomes = (row: LearningDomainType, column: LearningDomainType): LearningDomainOutcome[] => {
    return allOutcomes.value.filter(outcome => outcome.row.id == row.id && outcome.column?.id == column.id)
}
</script>

<style scoped lang="scss">
.competence-profile {
    box-sizing: border-box;
    table-layout: fixed;
    width: 750px;

    tr:last-of-type .competence-profile-header-row,
    tr:last-of-type .competence-profile-data {
        border-bottom: 1px solid #e0e0e0;
    }

    &-header {
        padding: 0.5rem;
        font-weight: 400;
        font-size: 0.9rem;

        &-col {
            border: 1px solid #e0e0e0;
            border-top: transparent;
            width: 6rem;
        }

        &-row {
            border: 1px none #e0e0e0;
            border-top-style: solid;
            display: flex;
        }

        &-color {
            margin: 3px 10px 0;
            width: 15px;
            height: 15px;
            font-size: 0.9rem;
        }
    }
}
</style>
