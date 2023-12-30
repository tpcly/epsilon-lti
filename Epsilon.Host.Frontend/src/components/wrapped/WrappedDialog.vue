<template>
	<v-dialog min-width="75%" min-height="75%" class="wrapped-dialog">
		<template #activator="{ props }">
			<v-btn v-bind="props"> {{ term?.name }} wrapped </v-btn>
		</template>

		<template #default="{ isActive }">
			<v-card>
				<v-toolbar>
					<v-toolbar-title>{{ term?.name }} wrapped</v-toolbar-title>

					<v-spacer></v-spacer>

					<v-btn icon @click="isActive.value = false">
						<v-icon>mdi-close</v-icon>
					</v-btn>
				</v-toolbar>
				<v-card-title> Masterd KPI's </v-card-title>
				<v-card-text>
					<v-row>
						<v-col cols="4">
							<v-card>
								<v-card-title>
									{{
										allOutcomesCurrentSemester?.filter(
											(o) =>
												allOutcomesPast.filter(
													(x) => x.id === o.id
												).length === 0
										).length
									}}
								</v-card-title>
								<v-card-text>
									New KPI's masterd this semester
								</v-card-text>
							</v-card>
						</v-col>
						<v-col cols="4">
							<v-card>
								<v-card-title>
									{{ allOutcomesCurrentSemester?.length }}
								</v-card-title>
								<v-card-text>
									Total amount of KPI's masterd this semester
								</v-card-text>
							</v-card>
						</v-col>
						<v-col cols="4">
							<v-card>
								<v-card-title>
									{{ allOutcomes.length }}
								</v-card-title>
								<v-card-text>
									Total amount of KPI's
								</v-card-text>
							</v-card>
						</v-col>
					</v-row>
				</v-card-text>
				<v-divider class="mt-4 mb-4"></v-divider>
				<v-card-title>Domain stats</v-card-title>
				<v-card-text>
					<v-row>
						<v-col
							v-for="domain in mostUsedDomains"
							:key="domain.domain.id"
							cols="6">
							<v-card>
								<v-card-text style="text-transform: uppercase">
									{{ domain.domain.id }}
								</v-card-text>
								<v-card-text>
									<v-row>
										<v-col :cols="domain.column ? 6 : 12">
											<ol>
												<li
													v-for="(i, x) in domain.row
														.sort(
															(a, b) =>
																b.count -
																a.count
														)
														.slice(0, 3)"
													:key="i.row.id">
													{{ x + 1 }}.
													{{ i?.row?.name }}:
													{{ i.count }}
												</li>
											</ol>
										</v-col>
										<v-col v-if="domain.column" cols="6">
											<ol>
												<li
													v-for="(
														i, x
													) in domain.column
														.sort(
															(a, b) =>
																b.count -
																a.count
														)
														.slice(0, 3)"
													:key="i.row.id">
													{{ x + 1 }}.
													{{ i?.row?.name }}:
													{{ i.count }}
												</li>
											</ol></v-col
										>
									</v-row>
								</v-card-text>
							</v-card>
						</v-col>
					</v-row>
				</v-card-text>
			</v-card>
		</template>
	</v-dialog>
</template>

<script setup lang="ts">
import type {
	EnrollmentTerm,
	LearningDomain,
	LearningDomainOutcome,
	LearningDomainSubmission,
	LearningDomainType,
} from "~/api.generated"

const props = defineProps<{
	submissions: LearningDomainSubmission[]
	domains: LearningDomain[]
	outcomes: LearningDomainOutcome[]
	terms: EnrollmentTerm[] | null
}>()

const term = computed<EnrollmentTerm | undefined>(() => props.terms?.at(0))

const allOutcomes = computed<LearningDomainOutcome[]>(() =>
	props.submissions.flatMap((submission) =>
		submission.results!.map((result) => result.outcome!)
	)
)

const allOutcomesPast = computed<LearningDomainOutcome[]>(() =>
	props.submissions
		.filter(
			(s) =>
				new Date(s.submittedAt!) <= new Date(term.value!.start_at ?? "")
		)
		.flatMap((submission) =>
			submission.results!.map((result) => result.outcome!)
		)
)

const allOutcomesCurrentSemester = computed<LearningDomainOutcome[]>(() =>
	props.submissions
		.filter(
			(s) =>
				new Date(s.submittedAt!) >=
					new Date(term.value!.start_at ?? "") &&
				new Date(s.submittedAt!) <= new Date(term.value!.end_at ?? "")
		)
		.flatMap((submission) =>
			submission.results!.map((result) => result.outcome!)
		)
)

const mostUsedDomains = computed<
	{
		domain: LearningDomain
		row: { count: number; row: LearningDomainType }[]
	}[]
>(() => {
	return props.domains.map((d) => {
		return {
			row: d.rowsSet?.types?.map((t) => {
				return {
					count: allOutcomesCurrentSemester.value?.filter(
						(o) => o.row?.id == t?.id
					).length,
					row: t,
				}
			}),
			column: d.columnsSet?.types?.map((t) => {
				return {
					count: allOutcomesCurrentSemester.value?.filter(
						(o) => o.column?.id == t?.id
					).length,
					row: t,
				}
			}),
			domain: d,
		}
	})
})
</script>

<style scoped lang="scss">
.wrapped-dialog .v-toolbar {
	background-color: #11284c;
	color: #ffffff;
}
</style>