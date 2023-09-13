<script setup lang="ts">
import { defineProps } from "vue"
import { LearningDomainOutcome } from "@/api"

defineProps<{
    result: LearningDomainOutcome | null
}>()
</script>

<template>
    <td v-if="result == null"></td>
    <td v-else :style="{ background: '#' + result?.value?.hexColor }"></td>
</template>

<style scoped lang="scss">
td {
    text-align: center;
}
</style>
