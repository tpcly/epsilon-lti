<template>
	<v-banner
		lines="one"
		icon="mdi-file"
		color="#ffffff"
		elevation="4"
		bg-color="#11284c">
		<v-banner-text> Download EduBadges</v-banner-text>

		<template #actions>
			<v-btn
				color="#fff"
				:loading="isDownloading"
				@click="downloadEdubadgeList">
				Download
			</v-btn>
		</template>
	</v-banner>
</template>

<script setup lang="ts">
import { useEpsilonStore } from "~/stores/use-store"

const api = useApi()
const store = useEpsilonStore()
const isDownloading = ref<boolean>(false)

function downloadEdubadgeList(): void {
	isDownloading.value = true
	api.document
		.documentDownloadCsvList([store.selectedUser?._id ?? ""], {
			from: store.selectedTermRange?.start.toDateString()!,
			to: store.selectedTermRange?.end.toDateString()!,
		})
		.then(async (response) => {
			const blob = await response.blob()
			const url = window.URL.createObjectURL(blob)
			const link = document.createElement("a")
			link.href = url
			link.setAttribute(
				"download",
				`EduBadge-List-${store.selectedUser?.name}.csv`
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
.v-banner {
	border-radius: 0.5rem;
}
</style>
