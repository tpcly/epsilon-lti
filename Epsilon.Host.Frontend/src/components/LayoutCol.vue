<template>
	<div :class="classes">
		<slot />
	</div>
</template>

<script setup lang="ts">
const componentProps = defineProps<{
	cols?: number | string
	breakpoints?: { [key: string]: number | string }
}>()

const classes = computed(() => {
	const classList: Array<string> = []

	if (componentProps.cols) {
		const typeOfCols = typeof componentProps.cols

		if (typeOfCols === "number" || typeOfCols === "string") {
			classList.push(`col-${componentProps.cols}`)
		}
	}

	if (componentProps.breakpoints) {
		for (const [breakpoint, cols] of Object.entries(
			componentProps.breakpoints
		)) {
			classList.push(`col-${breakpoint}-${cols}`)
		}
	}

	return classList
})
</script>

<style scoped lang="scss">
@import "src/assets/styles/mixins/breakpoint";

$row-distribution: 12;

@mixin distribute-column() {
	@for $i from 1 through $row-distribution {
		$percentage: calc(#{$i} / #{$row-distribution} * 100%);

		&-#{$i} {
			flex: 0 0 $percentage;
			max-width: $percentage;
		}
	}
}

.row > * {
	max-width: 100%;
	padding-right: calc(var(--bs-gutter-x) * 0.5);
	padding-left: calc(var(--bs-gutter-x) * 0.5);
	margin-top: var(--bs-gutter-y);
}

.col {
	@include breakpoints-labeled-default() {
		@include distribute-column;
	}

	&-auto {
		flex: 0 0 auto;
		width: auto;
		max-width: none;
	}
}
</style>
