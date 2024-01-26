<template>
	<table class="kpi-matrix">
		<thead>
			<tr>
				<th>
					<table>
						<tr>
							<td style="background-color: #44f656">Mastered</td>
						</tr>
						<tr>
							<td style="background-color: #fa1818">
								Not mastered
							</td>
						</tr>
						<tr>
							<td style="background-color: #9f2b68">
								Not Assessed
							</td>
						</tr>
						<tr>
							<td>1/2/3/4/5 Grade</td>
						</tr>
					</table>
				</th>
				<th
					v-for="submission of submissions"
					:key="submission.assignment"
					class="kpi-matrix-header">
					<a :href="submission.assignmentUrl" target="_blank">{{
						submission.assignment
					}}</a>
				</th>
			</tr>
		</thead>
		<tbody>
			<tr v-for="outcomeId of allOutcomes.sort()" :key="outcomeId">
				<th class="kpi-matrix-outcome">
					<div>
						{{ outcomes.find((o) => o.id == outcomeId).name }}
					</div>
				</th>
				<KpiMatrixCell
					v-for="submission of submissions"
					:key="submission?.assignmentUrl"
					class="kpi-matrix-cell"
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
import KpiMatrixCell from "~/components/competence/KpiMatrixCell.vue"
import {
	type LearningDomainOutcome,
	type LearningDomainSubmission,
} from "~/api.generated"

const componentProps = defineProps<{
	submissions: LearningDomainSubmission[]
	outcomes: LearningDomainOutcome[]
}>()

const allOutcomes = computed<number[]>(() => {
	let list: number[] = []
	componentProps.submissions.map((submission) =>
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
	&-header {
		text-decoration: underline;
		writing-mode: vertical-lr;
		-webkit-writing-mode: vertical-lr;
		padding: 10px;
	}

	&-cell {
		min-width: 25px;
	}

	&-outcome {
		width: 10px;
		border: 2px solid rgb(218, 219, 223);
		border-bottom: none;
		border-left: none;
		padding: 10px;
	}
}
</style>
