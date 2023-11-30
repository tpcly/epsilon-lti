<template>
	<table class="kpi-matrix">
		<thead>
			<tr>
				<th />
				<th
					v-for="submission of submissions"
					:key="submission.assignment"
					class="kpi-matrix-header kpi-matrix-header-assignment">
					{{ submission.assignment }}
				</th>
			</tr>
		</thead>
		<tbody>
			<tr
				v-for="outcome of allOutcomes.sort(
					(a, b) => a.value.order! - b.value.order!
				)"
				:key="outcome.id">
				<th
					:key="outcome.name"
					class="kpi-matrix-header kpi-matrix-header-outcome">
					<div>
						{{ outcome.name }}
					</div>
				</th>
				<KpiMatrixCell
					v-for="submission of submissions"
					:key="submission?.assignmentUrl"
					:result="
						submission.results?.find(
							(r) => r?.outcome?.id == outcome.id
						)
					"
					:criteria="
						submission.criteria?.find((c) => c?.id == outcome.id)
					">
				</KpiMatrixCell>
			</tr>
		</tbody>
	</table>
</template>

<script lang="ts" setup>
import { computed } from "vue"
import KpiMatrixCell from "~/components/competence/KpiMatrixCell.vue"
import {
	type LearningDomain,
	type LearningDomainOutcome,
	type LearningDomainSubmission,
} from "~/api.generated"

const props = defineProps<{
	domain: LearningDomain
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

<style lang="scss" scoped>
.kpi-matrix {
	display: block;
	max-width: -webkit-fill-available;
	overflow: auto;

	tr:last-of-type .kpi-matrix-header-assignment {
		border-bottom: 2px solid RGB(218, 219, 223, 0.7);
	}

	&-header {
		padding: 0.5rem;
		font-weight: 400;

		&-assignment {
			width: fit-content;
			border-bottom: 2px solid RGB(218, 219, 223, 0.7);
			border-right: 2px solid RGB(218, 219, 223, 0.7);
			border-left: 2px solid RGB(218, 219, 223, 0.7);
			writing-mode: vertical-rl;
			transform: rotate(180deg);
			padding: 10px;
		}

		&-outcome {
			min-width: max-content;
			border: 2px solid RGB(218, 219, 223, 0.7);
			border-bottom: none;
			border-left: none;
			display: flex;
			padding: 10px;
		}
	}
}
</style>
