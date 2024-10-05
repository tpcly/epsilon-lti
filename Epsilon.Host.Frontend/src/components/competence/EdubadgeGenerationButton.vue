<template>
	<v-btn :loading="isDownloading" @click="downloadEdubadgeList">
		Edu-badge create
	</v-btn>
</template>

<script setup lang="ts">
import { useEpsilonStore } from "~/stores/use-store"

const api = useApi()
const store = useEpsilonStore()
const isDownloading = ref<boolean>(false)

function downloadEdubadgeList(): void {
	isDownloading.value = true
	api.document
		.documentDownloadCsvList(store.users.map((u) => u._id) as string[], {
			from: store.selectedTermRange?.start.toDateString()!,
			to: store.selectedTermRange?.end.toDateString()!,
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
.v-banner {
	border-radius: 0.5rem;
}
</style>
