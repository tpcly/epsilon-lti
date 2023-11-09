<template>
	<div>
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
					</TabList>
				</div>
			</div>
			<hr class="divider mb-lg" />
			<main>
				<TabPanels>
					<TabPanel>
						<PerformanceDashboard
							:submissions="filteredSubmissions" />
					</TabPanel>
				</TabPanels>
			</main>
		</TabGroup>
	</div>
</template>

<script lang="ts" setup>
import { Tab, TabGroup, TabList, TabPanel, TabPanels } from "@headlessui/vue"
import TopNavigation from "~/components/TopNavigation.vue"
import { type LearningDomainSubmission, type User } from "~/api.generated"

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

const api = useApi()

const submissions = ref<LearningDomainSubmission[]>([])
const filterRange = ref<{ start: Date; end: Date } | null>(null)

const filteredSubmissions = computed(() => {
	const unwrappedFilterRange = filterRange.value

	if (!unwrappedFilterRange) {
		return submissions.value
	}

	return submissions.value.filter((submission) => {
		const submittedAt = new Date(submission.submittedAt!)

		return (
			submittedAt >= unwrappedFilterRange.start &&
			submittedAt <= unwrappedFilterRange.end
		)
	})
})

const handleUserChange = async (user: User): Promise<void> => {
	if (user._id === null) {
		return
	}

	const outcomesResponse = await api?.learning.outcomesList({
		studentId: user._id,
	})

	submissions.value = outcomesResponse.data
}

const handleRangeChange = (range: { start: Date; end: Date }): void => {
	filterRange.value = range
}
</script>

<style lang="scss" scoped>
.toolbar {
	display: flex;
	justify-content: space-between;

	&-slider {
		list-style: none;
		background-color: #f2f3f8;
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

			&:active,
			&:focus {
				outline: transparent;
			}

			&:hover {
				background-color: #d8d9dd;
			}

			&[data-headlessui-state="selected"] {
				background-color: white;
			}
		}
	}
}

.divider {
	border: 1px solid #f2f3f8;
}
</style>