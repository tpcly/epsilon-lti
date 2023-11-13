<template>
	<table class="kpi-matrix">
		<thead>
			<tr>
				<th></th>
				<th
					v-for="submission of store.state.filterdSubmissions"
					:key="submission.assignment">
					{{ submission.assignment }}
				</th>
			</tr>
		</thead>
		<tbody>
			<tr
				v-for="outcome of allOutcomes.sort() as string[]"
				:key="outcome">
				<th :key="outcome">
					{{ store.state.outcomes.find((o) => o.id == outcome).name }}
				</th>
				<KpiMatrixCell
					v-for="submission of store.state.filterdSubmissions"
					:key="submission.assignmentUrl"
					:result="
						submission.results?.find(
							(r) => r?.outcome?.id == outcome
						)
					"
					:criteria="
						submission.criteria?.find((c) => c?.id == outcome)
					"></KpiMatrixCell>
			</tr>
		</tbody>
	</table>
</template>
<script lang="ts" setup>
import { computed } from "vue"
import { useStore } from "vuex"
import KpiMatrixCell from "@/components/Competence/KpiMatrixCell.vue"

const store = useStore()

const allOutcomes = computed(() =>
	store.state.filterdSubmissions
		.flatMap((sub) => sub.criteria?.map((cr) => cr.id as string))
		.filter((value, index, self) => self.indexOf(value) === index)
)
</script>

<style lang="scss" scoped>
template {
	display: flex;
	flex-direction: column;
}

.kpi-matrix {
	margin-top: 3%;
	display: block;
	max-width: -webkit-fill-available;
	overflow: auto;
	margin-bottom: 3em;
}
tr {
	border: 2px solid RGB(218, 219, 223, 0.7);
}

tr td,
tr th {
	padding: 10px;
}

td div {
	border-bottom: 2px solid RGB(218, 219, 223, 0.7);
	width: 100%;
}

thead th {
	writing-mode: vertical-lr;
}
</style>
