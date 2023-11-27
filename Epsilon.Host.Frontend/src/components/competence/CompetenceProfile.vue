<template>
	<table class="competence-profile">
		<thead>
			<tr>
				<td />
				<th
					v-for="col of domain.columnsSet?.types"
					:key="col.id"
					class="competence-profile-header competence-profile-header-col">
					{{ col.name }}
				</th>
			</tr>
		</thead>
		<tbody>
			<tr v-for="row of domain.rowsSet.types" :key="row.id">
				<th
					class="competence-profile-header competence-profile-header-row">
					<div
						class="competence-profile-header-color"
						:style="{ backgroundColor: '#' + row.hexColor }" />
					{{ row.name }}
				</th>
				<CompetenceProfileCell
					v-for="col of domain.columnsSet?.types"
					:key="col.id"
					class="competence-profile-cell"
					:outcomes="outcomes(row, col)" />
			</tr>
		</tbody>
	</table>
</template>
<script setup lang="ts">
import { computed } from "vue"
import {
	type LearningDomain,
	type LearningDomainOutcome,
	type LearningDomainSubmission,
	type LearningDomainType,
} from "~/api.generated"
import CompetenceProfileCell from "~/components/competence/CompetenceProfileCell.vue"

const props = defineProps<{
	domain: LearningDomain
	submissions: LearningDomainSubmission[]
}>()

const allOutcomes = computed<LearningDomainOutcome[]>(() =>
	props.submissions.flatMap((submission) =>
		submission.results!.map((result) => result.outcome!)
	).filter((outcome, index, self) =>
		index === self.findIndex((t) => (
			t.id === outcome.id
		))
	)
)

const outcomes = (
	row: LearningDomainType,
	column: LearningDomainType
): LearningDomainOutcome[] => {
	return allOutcomes.value.filter(
		(outcome) => outcome.row.id == row.id && outcome.column?.id == column.id
	)
}
</script>

<style scoped lang="scss">
.competence-profile {
	border-collapse: collapse;
	width: 750px;

	tr:last-of-type .competence-profile-header-row {
		border-bottom: 2px solid RGB(218, 219, 223, 0.7);
	}

	&-header {
		padding: 0.5rem;
		font-weight: 400;
		font-size: 0.9rem;

		&-col {
			border-bottom: 2px solid RGB(218, 219, 223, 0.7);
			border-right: 2px solid RGB(218, 219, 223, 0.7);
			border-left: 2px solid RGB(218, 219, 223, 0.7);
			width: 6rem;
		}

		&-row {
			border: 2px solid RGB(218, 219, 223, 0.7);
			border-bottom: none;
			border-left: none;
			display: flex;
		}
	}
	&-row:first-child {
		border-right: 2px solid RGB(218, 219, 223, 0.7);
	}

	.profile-header-color {
		margin: 3px 10px 0;
		width: 15px;
		height: 15px;
		font-size: 0.9rem;
	}
}
</style>
