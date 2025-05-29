<template>
	<v-dialog
		:model-value="modelValue"
		width="500"
		:close-on-back="false"
		:persistent="true"
		attach=".v-window-item .v-window-item--active">
		<template #default>
			<v-card title="Loading">
				<v-card-text class="pa-5">
					<v-progress-linear indeterminate></v-progress-linear>
				</v-card-text>
			</v-card>
		</template>
	</v-dialog>
</template>

<script setup lang="ts">
defineProps<{
	modelValue: boolean
}>()
</script>

<style scoped lang="scss"></style>
