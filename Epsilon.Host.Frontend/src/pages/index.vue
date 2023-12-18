<template>
	<ClientOnly>
		<TopNavigation
			@user-change="handleUserChange"
			@range-change="handleRangeChange" />
		<TabGroup as="template">
			<div class="toolbar mb-lg mt-lg">
				<div class="toolbar-slider">
					<TabList>
						<Tab class="toolbar-slider-item">
							Performance dashboard
						</Tab>
						<Tab
							v-if="enableCompetenceProfile && domains.length > 1"
							class="toolbar-slider-item">
							Competence Document
						</Tab>
					</TabList>
				</div>
				<div v-if="enableCompetenceGeneration" class="toolbar-download">
					<Menu>
						<MenuButton @click="downloadCompetenceDocument">
							Download
						</MenuButton>
					</Menu>
				</div>
			</div>
			<hr class="divider mb-lg" />
			<main style="position: relative">
				<TabPanels>
					<TabPanel>
						<PerformanceDashboard
							:is-loading="loadingOutcomes"
							:submissions="filteredSubmissions"
							:domains="domains" />
					</TabPanel>
					<TabPanel>
						<CompetenceDocument
							:outcomes="outcomes"
							:submissions="filteredSubmissions"
							:filter-range="filterRange"
							:domains="domains" />
					</TabPanel>
				</TabPanels>
			</main>
		</TabGroup>
	</ClientOnly>
</template>

<script lang="ts" setup>
import {
	Tab,
	TabGroup,
	TabList,
	TabPanel,
	TabPanels,
	Menu,
	MenuButton,
} from "@headlessui/vue"
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
			console.log("Generating submissions")
			submissions.value = Generator.generateSubmissions(outcomes.value)
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

const filteredSubmissions = ref<LearningDomainSubmission[]>([])
watch(submissions, () => {
	const unwrappedFilterRange = filterRange.value

	if (!unwrappedFilterRange) {
		filteredSubmissions.value = submissions.value
	} else {
		filteredSubmissions.value = submissions.value.filter((submission) => {
			if (submission.criteria!.length > 0) {
				const submittedAt = new Date(submission.submittedAt!)

				return (
					submittedAt >= unwrappedFilterRange.startCorrected &&
					submittedAt <= unwrappedFilterRange.end
				)
			}
		})
	}
})

const handleUserChange = async (user: User): Promise<void> => {
	if (user._id === null) {
		return
	}
	console.log("Trigger new user index")
	currentUser.value = user
	loadingOutcomes.value = true

	await api?.learning
		.learningOutcomesList({
			studentId: user._id,
		})
		.then((r) => {
			// submissions.value = []
			submissions.value = r.data
			loadingOutcomes.value = false
			console.log("Results loaded")
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
	display: flex;
	justify-content: space-between;

	&-download {
		list-style: none;
		background-color: #11284c;
		padding: 5px;
		border-radius: 8px;
		font-size: 1em;

		button {
			border-radius: 5px;
			padding: 0.6em 1.2em;
			cursor: pointer;
			background-color: transparent;
			border: none;
			color: #ffffff;
			font-size: 1em;

			&:active,
			&:focus {
				outline: transparent;
			}

			&:hover {
				background-color: #d8d9dd;
				color: black;
			}
		}
	}

	&-slider {
		list-style: none;
		background-color: #11284c;
		padding: 5px;
		border-radius: 8px;
		width: fit-content;
		display: flex;
		align-items: center;
		justify-content: center;

		&-item {
			border-radius: 5px;
			padding: 0.6em 1.2em;
			cursor: pointer;
			background-color: transparent;
			border: none;
			font-size: 1em;
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

			&[data-headlessui-state="selected"] {
				background-color: white;
				color: black;
			}
		}
	}
}

.divider {
	border: 1px solid #f2f3f8;
}
</style>
