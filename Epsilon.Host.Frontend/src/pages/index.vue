<template>
	<ClientOnly>
		<TopNavigation
			@user-change="handleUserChange"
			@range-change="handleRangeChange">
			<template #default="navigationProps">
				<v-col
					v-if="enableSemesterWrapped && !loadingSubmissions"
					cols="12"
					md="2">
					<WrappedDialog
						:submissions="submissions"
						:outcomes="outcomes"
						:terms="navigationProps.terms"
						:domains="domains"></WrappedDialog>
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
					:domains="domains" />
			</v-window-item>
			<v-window-item v-if="enableCompetenceProfile" :value="1">
				<CompetenceGenerationBanner
					v-if="enableCompetenceGeneration"
					:filter-range="filterRange"
					:current-user="currentUser"></CompetenceGenerationBanner>
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
import { useEpsilonStore } from "~/composables/use-store"
import CompetenceGenerationBanner from "~/components/competence/CompetenceGenerationBanner.vue"

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
		enableSemesterWrapped.value = po.isFeatureEnabled("semester-wrapped")
		enableCompetenceProfile.value =
			po.isFeatureEnabled("competence-profile")
		enableCompetenceGeneration.value = po.isFeatureEnabled(
			"competence-generation"
		)
	})

	setInterval(() => {
		if (loadingSubmissions.value && outcomes.value.length > 0) {
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
		.catch((r) => store.addError(r))
	domainNames.map(function (domainName) {
		api.learning
			.learningDomainDetail(domainName)
			.then((hboIData) => {
				domains.value?.push(hboIData.data)
			})
			.catch((r) => store.addError(r))
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
	loadingSubmissions.value = true
	filterRange.value = null

	const response = await api?.learning.learningOutcomesList({
		studentId: user._id,
	})

	if (response.error) {
		store.addError(response.error)
	}

	submissions.value = response.data
	loadingSubmissions.value = false
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
