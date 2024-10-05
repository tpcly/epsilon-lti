// eslint-disable-next-line vue/no-mutating-props
<template>
	<table v-if="domain" class="competence-profile">
		<thead>
			<tr>
				<th class="w-0" />
				<th
					v-for="col of domain?.columnsSet?.types.sort(
						(a, b) => (a.order ?? 0) - (b.order ?? 0)
					)"
					:key="col.id"
					class="competence-profile-header competence-profile-header-col">
					<div class="d-sm-none text-center">
						{{ col.shortName }}
					</div>
					<div class="d-none d-sm-block text-center">
						{{ col.name }}
					</div>
				</th>
			</tr>
		</thead>
		<tbody>
			<tr
				v-for="row of domain?.rowsSet.types.sort(
					(a, b) => (a.order ?? 0) - (b.order ?? 0)
				)"
				:key="row.id">
				<th
					class="competence-profile-header competence-profile-header-row">
					<div
						class="competence-profile-header-color"
						:style="{ backgroundColor: '#' + row.hexColor }" />
					<div class="d-sm-none">
						{{ row.shortName }}
					</div>
					<div class="d-none d-sm-block">
						{{ row.name }}
					</div>
				</th>
				<CompetenceProfileCell
					v-for="col of domain?.columnsSet?.types.sort(
						(a, b) => (a.order ?? 0) - (b.order ?? 0)
					)"
					:key="col.id"
					:is-loading="isLoading"
					class="competence-profile-cell"
					:outcomes="outcomes(row, col)" />
			</tr>
		</tbody>
	</table>
	<table v-else class="competence-profile">
		<thead>
			<tr>
				<th />
				<th
					v-for="i of 5"
					:key="i"
					class="competence-profile-header competence-profile-header-col"></th>
			</tr>
		</thead>
		<tbody>
			<tr v-for="x of 5" :key="x">
				<th
					class="competence-profile-header competence-profile-header-row">
					<div
						class="competence-profile-header-color"
						:style="{ backgroundColor: '#11284C' }" />
				</th>
				<td v-for="i of 5" :key="i" class="competence-profile-cell" />
			</tr>
		</tbody>
	</table>
</template>
<script setup lang="ts">
import {
	type LearningDomain,
	type LearningDomainOutcome,
	type LearningDomainSubmission,
	type LearningDomainType,
} from "~/api.generated"
import CompetenceProfileCell from "~/components/competence/CompetenceProfileCell.vue"

const componentProps = defineProps<{
	domain: LearningDomain
	submissions: LearningDomainSubmission[]
	isLoading: boolean
}>()

const allOutcomes = computed<LearningDomainOutcome[]>(() =>
	componentProps.submissions.flatMap((submission) =>
		submission.results!.map((result) => result.outcome!)
	)
)

const outcomes = (
	row: LearningDomainType,
	column: LearningDomainType
): LearningDomainOutcome[] => {
	return allOutcomes.value.filter(
		(outcome) =>
			outcome?.row?.id == row.id && outcome?.column?.id == column.id
	)
}
</script>

<style scoped lang="scss">
.competence-profile {
	tr {
		border-bottom: 2px solid rgb(218, 219, 223);
	}

	tr td {
		border-right: 2px solid rgb(218, 219, 223);
		border-left: 2px solid rgb(218, 219, 223);
	}

	&-header {
		padding: 0.5rem;
		font-weight: 400;
		font-size: 0.9rem;

		&-col {
			border-right: 2px solid rgb(218, 219, 223);
			border-left: 2px solid rgb(218, 219, 223);
			width: 90px;
		}

		&-row {
			border-top-style: solid;
			display: flex;
			width: max-content;
		}

		&-color {
			margin: 3px 10px 0;
			width: 15px;
			height: 15px;
			font-size: 0.9rem;
		}
	}
}
</style>
