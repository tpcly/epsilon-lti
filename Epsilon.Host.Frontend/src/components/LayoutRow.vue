<template>
	<div class="row">
		<slot />
	</div>
</template>

<style scoped lang="scss">
.row {
	--bs-gutter-x: 1.5rem;
	--bs-gutter-y: 0;
	display: flex;
	flex-wrap: wrap;
	margin-top: calc(-1 * var(--bs-gutter-y));
	margin-right: calc(-0.5 * var(--bs-gutter-x));
	margin-left: calc(-0.5 * var(--bs-gutter-x));

	&.fill {
		height: 100%;
	}

	&.reverse {
		flex-direction: row-reverse;
	}
}
</style>
