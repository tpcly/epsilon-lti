<template>
	<ClientOnly>
		<TopNavigation
			@user-change="handleUserChange"
			@range-change="handleRangeChange">
			<template #default="navigationProps">
				<WrappedDialog
					v-if="!loadingOutcomes"
					:submissions="submissions"
					:outcomes="outcomes"
					:terms="navigationProps.terms"
					:domains="domains"></WrappedDialog>
			</template>
		</TopNavigation>
		<v-tabs v-model="tabs" class="toolbar">
			<div class="toolbar-items">
				<v-tab :value="0">Performance Dashboard</v-tab>
				<v-tab v-if="enableCompetenceProfile" :value="1">
					Competence Document
				</v-tab>
			</div>
			<v-spacer></v-spacer>

			<v-btn
				v-if="enableCompetenceGeneration"
				class="toolbar-download"
				@click="downloadCompetenceDocument">
				Download
			</v-btn>
		</v-tabs>
		<loading-dialog v-model="loadingOutcomes"></loading-dialog>
		<v-window v-model="tabs">
			<v-window-item :value="0">
				<PerformanceDashboard
					:is-loading="loadingOutcomes"
					:submissions="filteredSubmissions"
					:domains="domains" />
			</v-window-item>
			<v-window-item :value="1">
				<CompetenceDocument
					:outcomes="outcomes"
					:submissions="filteredSubmissions"
					:filter-range="filterRange"
					:domains="domains" />
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
import {
	type LearningDomain,
	type LearningDomainOutcome,
	type LearningDomainSubmission,
	type User,
} from "~/api.generated"
import { Posthog } from "~/utils/posthog"
import type { PostHog } from "posthog-js"
import PerformanceDashboard from "~/components/performance/PerformanceDashboard.vue"
import CompetenceDocument from "~/components/competence/CompetenceDocument.vue"
import { Generator } from "~/utils/generator"
import LoadingDialog from "~/LoadingDialog.vue"

const runtimeConfig = useRuntimeConfig()
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
const enableCompetenceProfile = ref<boolean | undefined>(false)
const enableCompetenceGeneration = ref<boolean | undefined>(false)
const api = useApi()
const loadingOutcomes = ref<boolean>(false)
const submissions = ref<LearningDomainSubmission[]>([])
const filterRange = ref<{
	start: Date
	end: Date
	startCorrected: Date
} | null>(null)
const currentUser = ref<User | null>(null)

const domains = ref<LearningDomain[]>([])
const outcomes = ref<LearningDomainOutcome[]>([])
if (process.client) {
	const po = Posthog.init() as PostHog
	po.onFeatureFlags(function () {
		enableCompetenceProfile.value =
			po.isFeatureEnabled("competence-profile")
		enableCompetenceGeneration.value = po.isFeatureEnabled(
			"competence-generation"
		)
	})

	setInterval(() => {
		if (loadingOutcomes.value) {
			filteredSubmissions.value = Generator.generateSubmissions(
				outcomes.value
			)
		}
	}, 1000)

	loadDomains(["hbo-i-2018", "pd-2020-bsc"])
}

function loadDomains(domainNames: string[]): void {
	api.learning
		.learningDomainOutcomesList()
		.then((r) => (outcomes.value = r.data))
	domainNames.map(function (domainName) {
		api.learning.learningDomainDetail(domainName).then((hboIData) => {
			domains.value?.push(hboIData.data)
		})
	})
}

function downloadCompetenceDocument(): void {
	api.document
		.documentDownloadWordList({
			userId: currentUser.value?._id as string,
			from: filterRange.value?.start.toDateString()!,
			to: filterRange.value?.end.toDateString()!,
		})
		.then(async (response) => {
			const blob = await response.blob()
			const url = window.URL.createObjectURL(blob)
			const link = document.createElement("a")
			link.href = url
			link.setAttribute("download", "competence-document.docx")
			document.body.appendChild(link)
			link.click()
		})
}

const filteredSubmissions = computed({
	get(): LearningDomainSubmission[] {
		const unwrappedFilterRange = filterRange.value

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

const handleUserChange = async (user: User): Promise<void> => {
	if (user._id === null) {
		return
	}
	currentUser.value = user
	loadingOutcomes.value = true
	filterRange.value = null

	await api?.learning
		.learningOutcomesList({
			studentId: user._id,
		})
		.then((r) => {
			submissions.value = r.data
			loadingOutcomes.value = false
		})
		.finally(() => {})
}

const handleRangeChange = (range: {
	start: Date
	end: Date
	startCorrected: Date
}): void => {
	filterRange.value = range
}
</script>

<style lang="scss" scoped>
.toolbar {
	height: unset;
	margin-top: 10px;

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

	.toolbar-items {
		background-color: #11284c;
		padding: 5px;
		border-radius: 8px;

		.v-btn.v-slide-group-item--active {
			background-color: white !important;
			color: black !important;
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
</style>
