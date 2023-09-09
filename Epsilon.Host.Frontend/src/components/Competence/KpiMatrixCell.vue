<script setup lang="ts">
import { defineProps } from "vue/dist/vue"
import { LearningDomainCriteria, LearningDomainOutcomeResult } from "@/api"
const props = defineProps<{
    result: LearningDomainOutcomeResult | undefined
    criteria: LearningDomainCriteria | undefined
}>()

function getColor(): string {
    if (
        props?.result?.grade != null &&
        props?.criteria?.masteryPoints != null
    ) {
        return (((props?.result?.grade as unknown as number) >=
            props?.criteria?.masteryPoints) as unknown as number)
            ? "#44F656"
            : "#FA1818"
    } else if (props.criteria?.masteryPoints != null) {
        return "#9F2B68"
    }
    return ""
}
</script>

<template>
    <td :style="{ backgroundColor: getColor() }"></td>
</template>

<style scoped lang="scss">
td {
    border: lightgray 0.5px solid;
}
</style>
