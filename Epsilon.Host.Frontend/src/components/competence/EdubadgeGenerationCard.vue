<template>
	<v-card>
		<v-toolbar>
			<v-toolbar-title> Edu-badge generation</v-toolbar-title>
		</v-toolbar>
		<v-card-text>
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
					<span>{{ item.title }}</span>
				</template>
			</v-autocomplete>
		</v-card-text>
		<v-card-text>
			<v-autocomplete
				v-model="selectedUser"
				label="Students"
				:items="store.users"
				density="compact"
				:clearable="true"
				:flat="true"
				item-value="id"
				item-title="name"
				return-object
				no-data-text>
			</v-autocomplete>
		</v-card-text>
		<v-card-actions>
			<v-spacer></v-spacer>
			<v-btn :loading="isDownloading" @click="downloadEdubadgeList">
				Edu-badge create
			</v-btn>
		</v-card-actions>
	</v-card>
</template>

<script setup lang="ts">
import { useEpsilonStore } from "~/stores/use-store"
import type { EnrollmentTerm, User } from "~/api.generated"
import { storeToRefs } from "pinia"
const store = useEpsilonStore()

const selectedTerm = ref<EnrollmentTerm | null>(null)
const selectedUser = ref<User | null>(store.users.at(0) as User)
const api = useApi()

const { terms } = storeToRefs(store)
watch(terms, () => {
	selectedTerm.value = terms.value.at(0) as EnrollmentTerm
})

const isDownloading = ref<boolean>(false)

function downloadEdubadgeList(): void {
	isDownloading.value = true
	api.document
		.documentDownloadEdubadgeCsvCreate([selectedUser.value?.name], {
			from: new Date(selectedTerm?.value?.startAt!).toDateString()!,
			to: new Date(selectedTerm.value?.endAt!).toDateString()!,
		})
		.then(async (response) => {
			const blob = await response.blob()
			const url = window.URL.createObjectURL(blob)
			const link = document.createElement("a")
			link.href = url
			link.setAttribute("download", `Edu-Badge.csv`)
			document.body.appendChild(link)
			link.click()
			isDownloading.value = false
		})
		.catch((r) => {
			isDownloading.value = false
			store.addError(r)
		})
}
</script>

<style scoped lang="scss">
.v-toolbar {
	background-color: #11284c;
	color: #ffffff;
}
</style>
