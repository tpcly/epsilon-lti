<template>
	{{
		allOutcomesCurrentSemester?.filter(
			(o) => allOutcomesPast.filter((x) => x.id === o.id).length === 0
		).length
	}}
	New KPI's <br />
	{{ allOutcomesCurrentSemester?.length }} Total received kpis this semester

	<br />Top 3

	<div v-for="domain in mostUsedRow" :key="domain.domain.id">
		<h1 style="text-transform: uppercase">{{ domain.domain.id }}</h1>
		<ol>
			<li
				v-for="(i, x) in domain.records
					.sort((a, b) => b.count - a.count)
					.slice(0, 3)"
				:key="i.row.id">
				{{ x + 1 }}. {{ i?.row?.name }}: {{ i.count }}
			</li>
		</ol>
	</div>
	<!--	<ol-->
	<!--		v-for="domain in mostUsedRow"-->
	<!--		:key="domain.domain.id"-->
	<!--		style="list-style: numberd">-->
	<!--		<li-->
	<!--			v-for="(i, x) in domain.records-->
	<!--				.sort((a, b) => b.count - a.count)-->
	<!--				.slice(0, 5)"-->
	<!--			:key="i.row.id">-->
	<!--			{{ x + 1 }}. {{ i?.row?.name }}: {{ i.count }}-->
	<!--		</li>-->
	<!--	</ol>-->
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

const allOutcomesPast = computed<LearningDomainOutcome[]>(() =>
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

const mostUsedRow = computed<
	{
		domain: LearningDomain
		records: { count: number; row: LearningDomainType }[]
	}[]
>(() => {
	return props.domains.map((d) => {
		return {
			records: d.rowsSet?.types?.map((t) => {
				return {
					count: allOutcomesCurrentSemester.value?.filter(
						(o) => o.row?.id == t?.id
					).length,
					row: t,
				}
			}),
			domain: d,
		}
	})
})
</script>

<style scoped lang="scss"></style>
