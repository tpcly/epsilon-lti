<template>
	<v-col cols="12" md="3">
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
	</v-col>
	<v-col cols="12" md="2">
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
				<span v-if="store.selectedTermRange.customSelection">
					Custom
				</span>
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
								label="From"
								:value="
									store.selectedTermRange.startCorrected
										?.toISOString()
										.slice(0, 10)
								"
								density="compact"
								type="date"
								@update:model-value="
									(s: string) => {
										store.setSelectedTermRange({
											customSelection: true,
											start: new Date(s),
											end: store.selectedTermRange.end,
											startCorrected: new Date(s),
										})
									}
								"></v-text-field>
						</v-list-item>
						<v-list-item>
							<v-text-field
								label="Until"
								:value="
									store.selectedTermRange.end
										?.toISOString()
										.slice(0, 10)
								"
								density="compact"
								type="date"
								@update:model-value="
									(s: string) => {
										store.setSelectedTermRange({
											customSelection: true,
											start: store.selectedTermRange
												.start,
											end: new Date(s),
											startCorrected:
												store.selectedTermRange
													.startCorrected,
										})
									}
								"></v-text-field>
						</v-list-item>
					</v-list>
				</v-menu>
			</template>
		</v-autocomplete>
	</v-col>
</template>

<script setup lang="ts">
import { useEpsilonStore } from "~/stores/use-store"
import { storeToRefs } from "pinia"
import { useServices } from "~/composables/use-services"
const store = useEpsilonStore()
const { selectedTerm, selectedUser } = storeToRefs(store)
onMounted(() => {
	useServices().loadStudents()
})

// When the user is updated, we should request its terms
watch(selectedUser, () => {
	store.setSelectedUser(selectedUser.value)
	useServices().loadTerms(store.selectedUser!)
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
