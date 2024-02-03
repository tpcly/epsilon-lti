<template>
	<td :style="{ backgroundColor: getColor() }">
		{{ result?.grade }}
	</td>
</template>
<script setup lang="ts">
import { defineProps } from "vue"
import type {
	LearningDomainCriteria,
	LearningDomainOutcomeRecord,
} from "@/api.generated"

const componentProps = defineProps<{
	result: LearningDomainOutcomeRecord | undefined
	criteria: LearningDomainCriteria | undefined
}>()

function getColor(): string {
	if (
		componentProps?.result?.grade != null &&
		componentProps?.criteria?.masteryPoints != null
	) {
		return (((componentProps?.result?.grade as unknown as number) >=
			componentProps?.criteria?.masteryPoints) as unknown as number)
			? "#44F656"
			: "#FA1818"
	} else if (componentProps.criteria?.masteryPoints != null) {
		return "#9F2B68"
	}
	return ""
}
</script>

<style scoped lang="scss">
td {
	text-align: center;
	border: 2px solid rgb(218, 219, 223);
}
</style>
