<template>
	<v-dialog min-width="75%" min-height="75%" class="wrapped-dialog">
		<template #activator="{ props }">
			<v-btn
				v-if="showWrapped"
				v-bind="props"
				class="wrappedButton"
				size="large">
				{{ term?.name }}<br />
				wrapped
			</v-btn>
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
				<v-card-title> Mastered KPI's</v-card-title>
				<v-card-text>
					<v-row>
						<v-col cols="12" sm="4">
							<v-card>
								<v-card-title>
									{{ newMasteredKpis.length }}
								</v-card-title>
								<v-card-text>
									New KPI's mastered this semester
								</v-card-text>
							</v-card>
						</v-col>
						<v-col cols="12" sm="4">
							<v-card>
								<v-card-title>
									{{ allOutcomesCurrentSemester?.length }}
								</v-card-title>
								<v-card-text>
									Total amount of KPI's mastered this semester
								</v-card-text>
							</v-card>
						</v-col>
						<v-col cols="12" sm="4">
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
				<v-card-title>Domain specific numbers</v-card-title>
				<v-card-text>
					<v-row>
						<v-col
							v-for="domain in mostUsedDomains"
							:key="domain.domain.id"
							cols="12"
							sm="6">
							<v-card>
								<v-card-text style="text-transform: uppercase">
									{{ domain.domain.id }}
								</v-card-text>
								<v-card-text>
									<v-row>
										<v-col
											:cols="12"
											:sm="domain.column ? 6 : 12">
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
										<v-col
											v-if="domain.column"
											cols="12"
											sm="6">
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
											</ol>
										</v-col>
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
	LearningDomainType,
} from "~/api.generated"
import { useEpsilonStore } from "~/stores/use-store"
const store = useEpsilonStore()
const term = computed<EnrollmentTerm | undefined>(() => store.terms?.at(0))
const showWrapped = computed<boolean>(() => {
	if (term.value != undefined) {
		return (
			(new Date(term.value!.endAt ?? "").getTime() -
				new Date().getTime()) /
				1000 /
				604800 <
			15
		)
	}

	return false
})
const allOutcomes = computed<LearningDomainOutcome[]>(() =>
	store.submissions.flatMap((submission) =>
		submission.results!.map((result) => result.outcome!)
	)
)

const allOutcomesPast = computed<LearningDomainOutcome[]>(() =>
	store.submissions
		.filter(
			(s) =>
				new Date(s.submittedAt!) <= new Date(term.value!.startAt ?? "")
		)
		.flatMap((submission) =>
			submission.results!.map((result) => result.outcome!)
		)
)

const allOutcomesCurrentSemester = computed<LearningDomainOutcome[]>(() =>
	store.submissions
		.filter(
			(s) =>
				new Date(s.submittedAt!) >=
					new Date(term.value!.startAt ?? "") &&
				new Date(s.submittedAt!) <= new Date(term.value!.endAt ?? "")
		)
		.flatMap((submission) =>
			submission.results!.map((result) => result.outcome!)
		)
)

const newMasteredKpis = computed<LearningDomainOutcome[]>(() =>
	allOutcomesCurrentSemester.value
		?.filter(
			(o) =>
				allOutcomesPast.value.filter((x) => x.id === o.id).length === 0
		)
		.filter(
			(outcome, index, self) =>
				index === self.findIndex((t) => t.id === outcome.id)
		)
)

const mostUsedDomains = computed<
	{
		domain: LearningDomain
		row: { count: number; row: LearningDomainType }[]
	}[]
>(() => {
	return store.domains.map((d) => {
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

/* Define keyframes for the animation */
@keyframes diagonalColorChange {
	0% {
		background-position: 0% 0%;
	}
	100% {
		background-position: 100% 100%;
	}
}

/* Apply styles to the button */
.wrappedButton {
	min-width: 100%;
	font-weight: unset;
	color: #fff;
	background: linear-gradient(45deg, #848da4, #11284c);
	background-size: 200% 200%;
	border: none;
	border-radius: 5px;
	cursor: pointer;
	animation: diagonalColorChange 1s infinite alternate;
	transition: background 0.3s ease-in-out;
}
</style>
