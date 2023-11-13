<template>
	<td :style="{ backgroundColor: getColor() }"></td>
</template>
<script setup lang="ts">
import { defineProps } from "vue/dist/vue"
import {
	LearningDomainCriteria,
	LearningDomainOutcomeRecord,
} from "@/api.generated"

const props = defineProps<{
	result: LearningDomainOutcomeRecord | undefined
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

<style scoped lang="scss">
td {
	text-align: center;
	border-bottom: 2px solid RGB(218, 219, 223, 0.7);
	border-right: 2px solid RGB(218, 219, 223, 0.7);
	border-top: 2px solid RGB(218, 219, 223, 0.7);
}
</style>
