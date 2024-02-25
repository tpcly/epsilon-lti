<template>
	<ClientOnly>
		<TopNavigation>
			<v-col v-if="!store.loadingSubmissions" cols="12" md="2">
				<WrappedDialog></WrappedDialog>
			</v-col>
			<ResultFiltering></ResultFiltering>
		</TopNavigation>
		<v-card v-if="store.errors.length" color="error" class="mt-4">
			<v-card-title>An error accord</v-card-title>
			<v-card-text>
				{{ store.errors.at(0) }}
			</v-card-text>
			<v-card-actions>
				<v-btn @click="router.go()"> Reload application</v-btn>
				<v-spacer></v-spacer>
				<v-btn
					:href="`https://github.com/tpcly/epsilon-lti/issues/new?title=${store.errors.at(0)}`"
					target="_blank">
					Report issue
				</v-btn>
			</v-card-actions>
		</v-card>
		<v-tabs v-model="tabs" class="toolbar mt-4" show-arrows>
			<v-tab :value="0">Performance Dashboard</v-tab>
			<v-tab :value="1"> Competence Document </v-tab>
		</v-tabs>
		<loading-dialog
			v-if="!store.errors.length"
			v-model="store.loadingSubmissions"></loading-dialog>

		<v-window v-model="tabs" class="mt-4">
			<v-window-item :value="0">
				<PerformanceDashboard />
			</v-window-item>
			<v-window-item :value="1">
				<CompetenceGenerationBanner />
				<CompetenceDocument />
			</v-window-item>
		</v-window>
		<div class="credits">
			<a class="version" :href="versionUrl" target="_blank">
				{{ runtimeConfig.public.clientVersion }}
			</a>
			| Epsilon © {{ new Date().getFullYear() }} |
			<a target="_blank" href="https://github.com/tpcly/epsilon-lti">
				GitHub
			</a>
		</div>
	</ClientOnly>
</template>

<script lang="ts" setup>
import TopNavigation from "~/components/TopNavigation.vue"
import { Posthog } from "~/utils/posthog"
import PerformanceDashboard from "~/components/performance/PerformanceDashboard.vue"
import CompetenceDocument from "~/components/competence/CompetenceDocument.vue"
import { Generator } from "~/utils/generator"
import LoadingDialog from "~/LoadingDialog.vue"
import CompetenceGenerationBanner from "~/components/competence/CompetenceGenerationBanner.vue"
import { storeToRefs } from "pinia"
import { useEpsilonStore } from "~/stores/use-store"
import { useServices } from "~/composables/use-services"

const runtimeConfig = useRuntimeConfig()
const store = useEpsilonStore()
const tabs = ref<number>(0)
const versionUrl =
	"https://github.com/tpcly/epsilon-lti/releases/tag/" +
	runtimeConfig.public.clientVersion
const { readCallback, validateCallback } = useLti()

const { data } = await useAsyncData(async () => {
	if (!process.server) {
		return
	}

	const event = useRequestEvent()
	return await readCallback(event)
})

if (process.client && data.value?.idToken) {
	const callback = data.value
	const validation = validateCallback(callback)

	if (validation) {
		useState("id_token", () => callback?.idToken)
	}
}
const router = useRouter()
const { selectedUser, selectedTermRange } = storeToRefs(store)
if (process.client) {
	Posthog.init()

	setInterval(() => {
		if (store.loadingSubmissions && store.outcomes.length > 0) {
			store.setFilteredSubmissions(
				Generator.generateSubmissions(store.outcomes)
			)
		}
	}, 1000)

	useServices().loadDomains(["hbo-i-2018", "pd-2020-bsc"])
}

watch(selectedUser, async () => useServices().loadSubmissions())
watch(selectedTermRange, () => useServices().filterSubmissions())
</script>

<style lang="scss" scoped>
.toolbar {
	background-color: #11284c;
	padding: 5px;
	border-radius: 8px;
	width: fit-content;
	height: unset;

	.v-btn.v-slide-group-item--active {
		background-color: white !important;
		color: black !important;
	}

	.v-btn {
		border-radius: 5px;
		padding: 0.6em 1.2em;
		cursor: pointer;
		letter-spacing: unset;
		background-color: #11284c;
		border: none;
		height: unset;
		font-size: 13px;
		text-transform: unset;
		color: #ffffff;

		&:active,
		&:focus {
			outline: transparent;
			color: black;
		}

		&:hover {
			background-color: #d8d9dd;
			color: black;
		}
	}
}

.divider {
	border: 1px solid #f2f3f8;
}

.credits,
.credits a {
	color: #0f254a;
}

.credits {
	padding: 20px;
	margin: 0 auto;
	display: block;
	width: 50%;
	text-align: center;
}

.v-tab.v-tab.v-btn {
	height: unset;
}
</style>
