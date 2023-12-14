<template>
	<div v-if="loading" class="dialog">
		<div class="dialog-container">{{ title }}</div>
	</div>
</template>

<script setup lang="ts">
defineProps<{
	loading: boolean
	title: string
}>()
</script>

<style lang="scss">
.dialog {
	position: absolute;
	left: 0;
	right: 0;
	top: 0;
	bottom: 0;
	background-color: rgba(0, 0, 0, 0.3);
	&-container {
		position: absolute;
		top: 45%;
		left: 25%;
		right: 25%;
		padding: 15px;
		background-color: #11284c;
		border-radius: 8px;
		color: #ffffff;
	}
}
</style>
