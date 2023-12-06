<template>
	<h2>Kpi-Matrix</h2>
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
			<tr v-for="outcomeId of allOutcomes.sort()" :key="outcomeId">
				<th class="kpi-matrix-header kpi-matrix-header-outcome">
					<div>
						{{ outcomes.find((o) => o.id == outcomeId).name }}
					</div>
				</th>
				<KpiMatrixCell
					v-for="submission of submissions"
					:key="submission?.assignmentUrl"
					:criteria="
						submission.criteria?.find((c) => c!.id == outcomeId)
					"
					:result="
						submission.results?.find(
							(r) => r?.outcome?.id == outcomeId
						)
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
	type LearningDomainOutcome,
	type LearningDomainSubmission,
} from "~/api.generated"

const props = defineProps<{
	submissions: LearningDomainSubmission[]
	outcomes: LearningDomainOutcome[]
}>()

const allOutcomes = computed<number[]>(() => {
	let list: number[] = []
	props.submissions.map((submission) =>
		submission.criteria!.map((result) => {
			list = list.filter((c) => c !== result.id)
			list.push(result.id as number)
		})
	)
	return list
})
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
