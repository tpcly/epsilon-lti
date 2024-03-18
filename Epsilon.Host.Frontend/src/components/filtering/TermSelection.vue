<template>
	<v-autocomplete
		v-model="selectedTerm"
		label="Semester"
		:items="store.terms"
		density="compact"
		:flat="true"
		item-title="name"
		return-object
		no-data-text>
		<template #selection="{ item }">
			<span v-if="store.selectedTermRange.customSelection"> Custom </span>
			<span v-else>{{ item.title }}</span>
		</template>
		<template #prepend-item>
			<v-menu :close-on-content-click="false">
				<template #activator="{ props }">
					<v-list-item v-bind="props"> Custom </v-list-item>
				</template>
				<v-list>
					<v-list-item>
						<v-text-field
							v-model="startDate"
							label="From"
							density="compact"
							type="date"></v-text-field>
					</v-list-item>
					<v-list-item>
						<v-text-field
							v-model="endDate"
							label="Until"
							density="compact"
							type="date"></v-text-field>
					</v-list-item>
					<v-list-item>
						<v-btn
							color="#11284c"
							variant="text"
							class="float-end"
							@click="applyCustomFilter">
							Apply
						</v-btn>
					</v-list-item>
				</v-list>
			</v-menu>
		</template>
	</v-autocomplete>
</template>

<script setup lang="ts">
import { useEpsilonStore } from "~/stores/use-store"
import { storeToRefs } from "pinia"
import { useServices } from "~/composables/use-services"
const store = useEpsilonStore()
const { selectedTerm, selectedTermRange } = storeToRefs(store)
const startDate = ref<string | null>()
const endDate = ref<string | null>()

function applyCustomFilter(): void {
	store.setSelectedTermRange({
		customSelection: true,
		start: new Date(startDate.value!),
		end: new Date(endDate.value!),
		startCorrected: new Date(startDate.value!),
	})
}
onMounted(() => {
	useServices().loadStudents()
})

watch(selectedTermRange, () => {
	startDate.value = selectedTermRange
		.value!.startCorrected?.toISOString()
		.slice(0, 10)
	endDate.value = selectedTermRange.value!.end?.toISOString().slice(0, 10)
})

watch(selectedTerm, () => {
	store.setSelectedTerm(selectedTerm.value)
	const selectedTermUnwrapped = store.selectedTerm
	if (!selectedTermUnwrapped?.startAt || !selectedTermUnwrapped.endAt) {
		return
	}

	store.setSelectedTermRange({
		customSelection: false,
		start: new Date(selectedTermUnwrapped?.startAt),
		end: new Date(selectedTermUnwrapped?.endAt),
		startCorrected: new Date(store.terms[store.terms.length - 1]?.startAt!),
	})
})
</script>

<style scoped lang="scss"></style>
