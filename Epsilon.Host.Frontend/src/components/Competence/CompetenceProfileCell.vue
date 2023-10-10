<template>
    <td v-if="result == null || count == 0"></td>
    <td v-else :style="{ background: '#' + result?.value?.hexColor }">
        {{ count }}
    </td>
</template>

<script setup lang="ts">
import { computed, defineProps } from "vue"
import {
    LearningDomainOutcome,
    LearningDomainSubmission,
    LearningDomainType,
} from "@/api"

const props = defineProps<{
    submissions: LearningDomainSubmission[]
    result: LearningDomainOutcome | null
    row: LearningDomainType | null
    col: LearningDomainType | null
}>()
/*
 * Count all the outcomes that have been graded to mastery within a row and column
 */
const count = computed(() => {
    return props.submissions
        .map(
            (s) =>
                s.results?.filter((r) => {
                    if (
                        r.outcome?.row?.id != null &&
                        r.outcome?.column?.id != null &&
                        props.row?.id != null &&
                        props.col?.id != null
                    ) {
                        return (
                            r.outcome.row.id == props.row.id &&
                            r.outcome.column.id == props.col.id &&
                            (r.grade as number) >= 3
                        )
                    }
                    return false
                }).length
        )
        .reduce((sum, current) => {
            return (sum as number) + (current ?? 0)
        }, 0)
})
</script>

<style scoped lang="scss">
td {
    text-align: center;
    border-bottom: 1px solid black;
    border-right: 1px solid black;
}
</style>
