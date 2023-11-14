<template>
	<table class="kpi-table">
		<tr 
		v-for="outcomeId of allOutcomes.sort()" :key="outcomeId" class="kpi-table-outcome">
			<th class="kpi-table-outcome kpi-table-outcome-name">
				{{ store.state.outcomes.find((o) => o.id === outcomeId).name }}
			</th>
			<td class="kpi-table-outcome kpi-table-outcome-submission">
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
					<span>{{
						submission.results.find(
							(r) => r.outcome.id === outcomeId
						)?.grade
					}}</span>
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
	border-collapse: collapse;
	
	&-outcome {
		border-bottom: 2px solid RGB(218, 219, 223, 0.7);
		padding: 10px;

		&-name {
			width: 200px;
			border-right: 2px solid RGB(218, 219, 223, 0.7);
		}
		&-submission{
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
