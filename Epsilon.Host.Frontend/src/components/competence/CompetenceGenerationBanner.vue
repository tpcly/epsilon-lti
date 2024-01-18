<template>
	<v-banner
		lines="one"
		icon="mdi-file"
		color="#ffffff"
		elevation="4"
		bg-color="#11284c">
		<v-banner-text> Download your competence document </v-banner-text>

		<template #actions>
			<v-btn
				color="#fff"
				:loading="isDownloading"
				@click="downloadCompetenceDocument">
				Download
			</v-btn>
		</template>
	</v-banner>
</template>

<script setup lang="ts">
import type { User } from "~/api.generated"

const props = defineProps<{
	currentUser: User | null
	filterRange: {
		start: Date
		end: Date
		startCorrected: Date
	} | null
}>()
const api = useApi()
const store = useEpsilonStore()
const isDownloading = ref<boolean>(false)
function downloadCompetenceDocument(): void {
	isDownloading.value = true
	api.document
		.documentDownloadWordList({
			userId: props.currentUser?._id as string,
			from: props.filterRange?.start.toDateString()!,
			to: props.filterRange?.end.toDateString()!,
		})
		.then(async (response) => {
			const blob = await response.blob()
			const url = window.URL.createObjectURL(blob)
			const link = document.createElement("a")
			link.href = url
			link.setAttribute(
				"download",
				`CD-Epsilon-${props.currentUser?.name}.docx`
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
