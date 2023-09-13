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
    row: LearningDomainType
    col: LearningDomainType
}>()

const count = computed(() => {
    return props.submissions
        .map(
            (s) =>
                s.results?.filter(
                    (r) =>
                        r.outcome?.row?.id == props.row.id &&
                        r.outcome?.column?.id == props.col.id &&
                        (r.grade as number) >= 3
                ).length
        )
        .reduce((sum, current) => {
            if (typeof current === "number") {
                return (sum as number) + current
            }
            return sum
        }, 0)
})
</script>

<style scoped lang="scss">
td {
    text-align: center;
}
</style>
