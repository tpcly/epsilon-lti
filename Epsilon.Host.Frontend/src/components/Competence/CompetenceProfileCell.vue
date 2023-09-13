<script setup lang="ts">
import { defineProps } from "vue"
import { LearningDomainOutcome } from "@/api"

defineProps<{
    result: LearningDomainOutcome | null
    count: number
}>()
</script>

<template>
    <td v-if="result == null || count == 0"></td>
    <td v-else :style="{ background: '#' + result?.value?.hexColor }">
        {{ count }}
    </td>
</template>

<style scoped lang="scss">
td {
    text-align: center;
}
</style>
