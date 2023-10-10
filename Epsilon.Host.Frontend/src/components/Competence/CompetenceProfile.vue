<template>
	<table class="competence-profile">
		<thead>
			<tr>
				<td />
				<th
					v-for="col of store.state.domain.columnsSet?.types"
					:key="col.id"
					class="competence-profile-header competence-profile-header-col">
					{{ col.name }}
				</th>
			</tr>
		</thead>
		<tbody>
			<tr v-for="row of store.state.domain.rowsSet?.types" :key="row.id">
				<th
					class="competence-profile-header competence-profile-header-row">
					<div
						class="profile-header-color"
						:style="{
							backgroundColor: '#' + row.hexColor,
						}"></div>
					{{ row.name }}
				</th>
				<CompetenceProfileCell
					v-for="col of store.state.domain.columnsSet?.types"
					:key="col.id"
					:col="col"
					:row="row"
					:submissions="submissions"
					:result="getFiltered(row, col, allOutcomes)">
				</CompetenceProfileCell>
			</tr>
		</tbody>
	</table>
</template>

<script setup lang="ts">
import { useStore } from "vuex"

const store = useStore()
import { computed } from "vue"
import {
	LearningDomainOutcome,
	LearningDomainSubmission,
	LearningDomainType,
} from "@/api.generated"
import CompetenceProfileCell from "@/components/Competence/CompetenceProfileCell.vue"

const props = defineProps<{
	submissions: LearningDomainSubmission[]
}>()

const allOutcomes = computed(() =>
	props.submissions
		.flatMap((sub) => sub.criteria?.map((cr) => cr.id))
		.filter((value, index, self) => self.indexOf(value) === index)
) as unknown as number[]

function getFiltered(
	row: LearningDomainType,
	col: LearningDomainType,
	outcomes: number[]
): LearningDomainOutcome {
	return store.state.outcomes
		.filter(
			(o: LearningDomainOutcome) =>
				o?.row?.id === row.id &&
				o?.column?.id === col.id &&
				outcomes.includes(o.id as number)
		)
		.reduce((acc: LearningDomainOutcome, value: LearningDomainOutcome) => {
			return (acc?.value?.shortName as unknown as number) >
				(value?.value?.shortName as unknown as number)
				? acc
				: value
		}, null)
}
</script>

<style scoped lang="scss">
.competence-profile {
	border-collapse: collapse;
	width: 750px;

	tr:last-of-type .competence-profile-header-row,
	tr:last-of-type .competence-profile-data {
		border-bottom: 1px solid #e6e6e6;
	}

	&-header {
		padding: 0.5rem;
		font-weight: 400;
		font-size: 0.9rem;

		&-col {
			border: 1px solid #e6e6e6;
			border-top: transparent;
			width: 6rem;
		}

		&-row {
			border: 1px solid #e6e6e6;
			border-bottom: none;
			border-left: none;
			display: flex;
		}
	}

	&-data:last-child {
		border-right: 1px solid #e6e6e6;
	}

	.profile-header-color {
		margin: 3px 10px 0;
		width: 15px;
		height: 15px;
		font-size: 0.9rem;
		border: 1px solid #e6e6e6;
	}
}
</style>
