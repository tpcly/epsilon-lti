<template>
	<table class="kpi-matrix">
		<thead>
			<tr>
				<th />
				<th
					v-for="submission of store.state.filterdSubmissions"
					:key="submission.assignment"
					class="kpi-matrix-header kpi-matrix-header-assignment">
					{{ submission.assignment }}
				</th>
			</tr>
		</thead>
		<tbody>
			<tr
				v-for="outcome of allOutcomes.sort()" :key="outcome">
				<th :key="outcome" class="kpi-matrix-header kpi-matrix-header-outcome">
					<div>
						{{ store.state.outcomes.find((o) => o.id == outcome).name }}
					</div>
				</th>
				<KpiMatrixCell
					v-for="submission of store.state.filterdSubmissions"
					:key="submission.assignmentUrl"
					:result="
						submission.results?.find(
							(r) => r?.outcome?.id == outcome
						)
					"
					:criteria="submission.criteria?.find((c) => c?.id == outcome)">
				</KpiMatrixCell>
			</tr>
		</tbody>
	</table>
</template>
<script lang="ts" setup>
import { computed } from "vue"
import { useStore } from "vuex"
import KpiMatrixCell from "~/components/competence/KpiMatrixCell.vue"

const store = useStore()

const allOutcomes = computed(() =>
	store.state.filterdSubmissions
		.flatMap((sub) => sub.criteria?.map((cr) => cr.id as string))
		.filter((value, index, self) => self.indexOf(value) === index)
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
		padding: .5rem;
		font-weight: 400;

		&-assignment {
			border-bottom: 2px solid RGB(218, 219, 223, 0.7);
			border-right: 2px solid RGB(218, 219, 223, 0.7);
			border-left: 2px solid RGB(218, 219, 223, 0.7);
			writing-mode: vertical-rl;
			transform: rotate(180deg);
			padding: 10px;
		}

		&-outcome {
			border: 2px solid RGB(218, 219, 223, 0.7);
			border-bottom: none;
			border-left: none;
			display: flex;
			padding: 10px;
		}
	}
}
</style>
