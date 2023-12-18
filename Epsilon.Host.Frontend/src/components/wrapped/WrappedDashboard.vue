<template>
	{{
		allOutcomesCurrentSemester?.filter(
			(o) => allOutcomes.filter((x) => x.id === o.id).length === 0
		)?.length
	}}
	{{ allOutcomesCurrentSemester?.length }}
	{{ allOutcomesCurrentSemester. }}
</template>

<script setup lang="ts">
import type {
	LearningDomain,
	LearningDomainOutcome,
	LearningDomainSubmission,
	LearningDomainType,
} from "~/api.generated"

const props = defineProps<{
	submissions: LearningDomainSubmission[]
	domains: LearningDomain[]
	outcomes: LearningDomainOutcome[]
	filterRange: {
		start: Date
		end: Date
		startCorrected: Date
	} | null
}>()

const allOutcomes = computed<LearningDomainOutcome[]>(() =>
	props.submissions
		.filter((s) => new Date(s.submittedAt!) <= props.filterRange!.start)
		.flatMap((submission) =>
			submission.results!.map((result) => result.outcome!)
		)
)

const allOutcomesCurrentSemester = computed<LearningDomainOutcome[]>(() =>
	props.submissions
		.filter(
			(s) =>
				new Date(s.submittedAt!) >= props.filterRange!.start &&
				new Date(s.submittedAt!) <= props.filterRange!.end
		)
		.flatMap((submission) =>
			submission.results!.map((result) => result.outcome!)
		)
)

// const mostUsedRow = computed<LearningDomainType>(() => {
// 	props.domains.map((d) => {
// 		return {
// 			count: allOutcomesCurrentSemester.value.filter(
// 				(o) => o.row.id === d.rowsSet.id
// 			).length,
// 			row: d.rowsSet,
// 		}
// 	})
// })
</script>

<style scoped lang="scss"></style>
