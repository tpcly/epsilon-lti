<template>
	<table class="kpi-table">
		<tr v-for="outcomeId of allOutcomes.sort()" :key="outcomeId">
			<th>
				{{ store.state.outcomes.find((o) => o.id === outcomeId).name }}
			</th>
			<td>
				<div
					v-for="submission of store.state.filterdSubmissions.filter(
						(s) => {
							if (s.results != null) {
								return (
									s.results.filter(
										(r) => r?.outcome?.id == outcomeId
									).length > 0
								)
							}
						}
					)"
					:key="submission.submittedAt">
					<a :href="submission.assignmentUrl" target="_blank">{{
						submission.assignment
					}}</a>
				</div>
			</td>
			<td>
				<div
					v-for="submission of store.state.filterdSubmissions.filter(
						(s) => {
							if (s.results != null) {
								return (
									s.results.filter(
										(r) => r?.outcome?.id == outcomeId
									).length > 0
								)
							}
						}
					)"
					:key="submission.submittedAt">
					{{
						submission.results.find(
							(r) => r.outcome.id === outcomeId
						)?.grade
					}}
				</div>
			</td>
		</tr>
	</table>
</template>
<script setup lang="ts">
import { useStore } from "vuex"
import { computed } from "vue"

const store = useStore()

const allOutcomes = computed(() =>
	store.state.filterdSubmissions
		.flatMap((sub) => sub.results?.map((r) => parseInt(r.outcome?.id)))
		.filter((value, index, self) => self.indexOf(value) === index)
) as unknown as number[]
</script>

<style scoped lang="scss">
template {
	display: flex;
	flex-direction: column;
}

.kpi-table {
	display: block;
	overflow: auto;
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
</style>
