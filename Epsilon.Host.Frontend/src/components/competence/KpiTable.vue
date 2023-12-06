<template>
	<h2>Kpi-Table</h2>
	<table class="kpi-table">
		<tr
			v-for="outcome of allOutcomes.sort(
				(a, b) => a.value.order! - b.value.order!
			)"
			:key="outcome.id"
			class="kpi-table-outcome">
			<th class="kpi-table-outcome kpi-table-outcome-name">
				{{ outcome.name }}
			</th>
			<td class="kpi-table-outcome kpi-table-outcome-submission">
				<div
					v-for="submission of submissions.filter((s) => {
						if (s.results != null) {
							return (
								s.results.filter(
									(r) => r?.outcome?.id == outcome.id
								).length > 0
							)
						}
					})"
					:key="submission.submittedAt">
					<a :href="submission?.assignmentUrl" target="_blank">
						{{ submission.assignment }}</a
					>
					<span>{{
						submission.results?.find(
							(r) => r.outcome?.id == outcome.id
						)?.grade
					}}</span>
				</div>
			</td>
		</tr>
	</table>
</template>

<script setup lang="ts">
import { computed } from "vue"
import type {
	LearningDomainOutcome,
	LearningDomainSubmission,
} from "~/api.generated"

const props = defineProps<{
	outcomes: LearningDomainOutcome[]
	submissions: LearningDomainSubmission[]
}>()

const allOutcomes = computed<LearningDomainOutcome[]>(() =>
	props.submissions
		.flatMap((submission) =>
			submission.results!.map((result) => result.outcome!)
		)
		.filter(
			(outcome, index, self) =>
				index === self.findIndex((t) => t.id === outcome.id)
		)
)
</script>

<style scoped lang="scss">
template {
	display: flex;
	flex-direction: column;
}

.kpi-table {
	display: block;
	overflow: auto;
	border-collapse: collapse;

	&-outcome {
		border-bottom: 2px solid RGB(218, 219, 223, 0.7);
		padding: 10px;

		&-name {
			width: 200px;
			border-right: 2px solid RGB(218, 219, 223, 0.7);
		}
		&-submission {
			width: 400px;
			div {
				display: flex;
				justify-content: space-between;
				a {
					white-space: nowrap;
					overflow: hidden;
					text-overflow: ellipsis;
					max-width: 95%;
				}
			}
		}
	}
}

td div {
	border-bottom: 2px solid RGB(218, 219, 223, 0.7);
}
</style>
