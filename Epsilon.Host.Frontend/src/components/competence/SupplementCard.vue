<template>
	<v-card>
		<v-toolbar>
			<v-toolbar-title>Supplement document generation</v-toolbar-title>
		</v-toolbar>
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
			<v-btn :loading="isDownloading" @click="downloadSupplement">
				Supplement create
			</v-btn>
		</v-card-actions>
	</v-card>
</template>

<script setup lang="ts">
import { useEpsilonStore } from "~/stores/use-store"
import type { User } from "~/api.generated"
const store = useEpsilonStore()

const selectedUser = ref<User | null>(store.users.at(0) as User)
const api = useApi()

const isDownloading = ref<boolean>(false)

function downloadSupplement(): void {
	isDownloading.value = true
	api.document
		.documentDownloadSupplementList({
			userId: selectedUser.value?.id ?? "",
			domains: store.usedDomains.join(","),
		})
		.then(async (response) => {
			const blob = await response.blob()
			const url = window.URL.createObjectURL(blob)
			const link = document.createElement("a")
			link.href = url
			link.setAttribute(
				"download",
				`Supplement-Document-${selectedUser.value?.name}.docx`
			)
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
