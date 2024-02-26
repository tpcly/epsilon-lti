<template>
	<v-autocomplete
		v-model="selectedUser"
		label="Students"
		:items="store.users"
		density="compact"
		:clearable="true"
		:flat="true"
		item-value="_id"
		item-title="name"
		return-object
		no-data-text
		@update:model-value="(e) => store.setSelectedUser(e)">
	</v-autocomplete>
</template>

<script setup lang="ts">
import { useServices } from "~/composables/use-services"
import { useEpsilonStore } from "~/stores/use-store"
import { storeToRefs } from "pinia"
const store = useEpsilonStore()
const { selectedUser } = storeToRefs(store)
watch(selectedUser, () => {
	store.setSelectedUser(selectedUser.value)
	useServices().loadTerms(store.selectedUser!)
})
</script>

<style scoped lang="scss"></style>
