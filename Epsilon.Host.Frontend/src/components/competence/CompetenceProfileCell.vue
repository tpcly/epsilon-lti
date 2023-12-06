<template>
	<td v-if="value" :style="{ background: '#' + value.hexColor }">
		{{ outcomes.length }}
	</td>
	<td v-else class="border" />
</template>

<script setup lang="ts">
import {
	type LearningDomainOutcome,
	type LearningDomainType,
} from "~/api.generated"

const props = defineProps<{
	outcomes: LearningDomainOutcome[]
}>()

const value = computed<LearningDomainType | null>(() => {
	const types = props.outcomes.map((outcome) => outcome.value)
	types.sort((a, b) => (b.order ?? 0) - (a.order ?? 0))

	return types[0]
})
</script>

<style scoped lang="scss">
td {
	text-align: center;
	border-bottom: 2px solid rgb(218, 219, 223);
	border-right: 2px solid rgb(218, 219, 223);
	border-top: 2px solid rgb(218, 219, 223);
	&.border {
		border: 1px solid #e6e6e6;
	}
}
</style>
