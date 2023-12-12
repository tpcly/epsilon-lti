<template>
	<div v-if="submissions" class="competence-document">
		<KpiTable
			:outcomes="allOutcomes"
			:submissions="filteredSubmissionsDateSelection" />
		<h2>Competence Profile</h2>
		<CompetenceProfile
			:submissions="filteredSubmissionsDateSelection"
			:domain="domains.find((l) => l.id == 'hbo-i-2018')" />
		<KpiMatrix
			v-if="outcomes.length > 0"
			:outcomes="outcomes"
			:submissions="filteredSubmissionsDateSelection" />
	</div>
</template>

<script lang="ts" setup>
import KpiMatrix from "~/components/competence/KpiMatrix.vue"
import KpiTable from "~/components/competence/KpiTable.vue"
import CompetenceProfile from "~/components/competence/CompetenceProfile.vue"
import type {
	LearningDomain,
	LearningDomainOutcome,
	LearningDomainSubmission,
} from "~/api.generated"

const props = defineProps<{
	submissions: LearningDomainSubmission[]
	domains: LearningDomain[]
	filterRange: {
		start: Date
		end: Date
		startCorrected: Date
	} | null
}>()

const outcomes = ref<LearningDomainOutcome[]>([])

const filteredSubmissionsDateSelection = computed(() => {
	const unwrappedFilterRange = props.filterRange

	if (!unwrappedFilterRange) {
		return props.submissions
	}

	return props.submissions.filter((submission) => {
		if (submission.criteria!.length > 0) {
			const submittedAt = new Date(submission.submittedAt!)

			return (
				submittedAt >= unwrappedFilterRange.start &&
				submittedAt <= unwrappedFilterRange.end
			)
		}
	})
})

const api = useApi()
api.learning.learningDomainOutcomesList().then((r) => (outcomes.value = r.data))

const data = api.document.documentDownloadWordList({
  userId: "27381",
  from: props.filterRange?.start.toDateString()!,
  to: props.filterRange?.end.toDateString()!,
}).then(async (response) => {
  const blob = await response.blob();
  const url = window.URL.createObjectURL(blob);
  const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', 'competence-document.docx');
    document.body.appendChild(link);
    link.click();
});

const allOutcomes = computed<LearningDomainOutcome[]>(() =>
	props.submissions.flatMap((submission) =>
		submission.results!.map((result) => result.outcome!)
	)
)
</script>

<style scoped>
.competence-document {
	display: grid;
	gap: 2rem 0;
}
</style>
