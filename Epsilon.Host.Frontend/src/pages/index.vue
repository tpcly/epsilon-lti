<template>
	<ClientOnly>
		<TopNavigation>
			<template>
				<v-col
					v-if="enableSemesterWrapped && !loadingSubmissions"
					cols="12"
					md="2">
					<WrappedDialog></WrappedDialog>
				</v-col>
			</template>
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
			<v-tab v-if="enableCompetenceProfile" :value="1">
				Competence Document
			</v-tab>
		</v-tabs>
		<loading-dialog
			v-if="!store.errors.length"
			v-model="loadingSubmissions"></loading-dialog>

		<v-window v-model="tabs" class="mt-4">
			<v-window-item :value="0">
				<PerformanceDashboard
					:is-loading="loadingSubmissions"
					:submissions="filteredSubmissions"
					:domains="store.domains" />
			</v-window-item>
			<v-window-item v-if="enableCompetenceProfile" :value="1">
				<CompetenceGenerationBanner
					v-if="enableCompetenceGeneration"
					:filter-range="store.selectedTermRange"
					:current-user="
						store.selectedUser
					"></CompetenceGenerationBanner>
				<CompetenceDocument
					:outcomes="store.outcomes"
					:submissions="filteredSubmissions"
					:filter-range="store.selectedTermRange"
					:domains="store.domains" />
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
import { type LearningDomainSubmission } from "~/api.generated"
import { Posthog } from "~/utils/posthog"
import type { PostHog } from "posthog-js"
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
const enableCompetenceProfile = ref<boolean | undefined>(false)
const enableCompetenceGeneration = ref<boolean | undefined>(false)
const enableSemesterWrapped = ref<boolean | undefined>(false)
const api = useApi()
const loadingSubmissions = ref<boolean>(true)
const submissions = ref<LearningDomainSubmission[]>([])
const { selectedUser } = storeToRefs(store)
if (process.client) {
	Posthog.init()

	setInterval(() => {
		if (loadingSubmissions.value && store.outcomes.length > 0) {
			filteredSubmissions.value = Generator.generateSubmissions(
				store.outcomes
			)
		}
	}, 1000)

	useServices().loadDomains(["hbo-i-2018", "pd-2020-bsc"])
}

const filteredSubmissions = computed({
	get(): LearningDomainSubmission[] {
		const unwrappedFilterRange = store.selectedTermRange

		if (!unwrappedFilterRange) {
			return submissions.value
		}

		return submissions.value.filter((submission) => {
			if (submission.criteria!.length > 0) {
				const submittedAt = new Date(submission.submittedAt!)

				return (
					submittedAt >= unwrappedFilterRange.startCorrected &&
					submittedAt <= unwrappedFilterRange.end
				)
			}
		})
	},
	set(values: LearningDomainSubmission[]) {
		submissions.value = values
	},
})

watch(selectedUser, async () => {
	if (store.selectedUser?._id === null) {
		return
	}
	loadingSubmissions.value = true
	// filterRange.value = null

	const response = await api?.learning.learningOutcomesList({
		studentId: store.selectedUser!._id,
	})

	if (response.error) {
		store.addError(response.error)
	}

	submissions.value = response.data
	loadingSubmissions.value = false
})
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
