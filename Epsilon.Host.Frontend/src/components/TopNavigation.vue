<template>
	<div class="top-navigation">
		<v-row>
			<v-col cols="12" md="4">
				<a href="https://github.com/tpcly/epsilon-lti" target="_blank">
					<img
						alt="logo"
						class="top-navigation-logo"
						src="../assets/logo-white.png" />
					<div
						v-if="
							runtimeConfig.public.clientVersion.includes('Beta')
						"
						class="top-navigation-beta">
						Beta
					</div>
				</a>
			</v-col>
			<v-spacer></v-spacer>
			<slot :terms="terms"></slot>
			<v-col cols="12" md="3">
				<v-autocomplete
					v-model="selectedUser"
					label="Students"
					:items="users"
					density="compact"
					:flat="true"
					item-value="_id"
					item-title="name"
					return-object
					no-data-text>
				</v-autocomplete>
			</v-col>
			<v-col cols="12" md="2">
				<v-autocomplete
					v-model="selectedTerm"
					label="Semester"
					:items="terms"
					density="compact"
					:flat="true"
					item-title="name"
					return-object
					no-data-text>
				</v-autocomplete>
			</v-col>
		</v-row>
	</div>
</template>

<script lang="ts" setup>
import { type EnrollmentTerm, type User } from "~/api.generated"

const emit = defineEmits(["userChange", "rangeChange"])
const api = useApi()

const users = ref<User[]>([])
const terms = ref<EnrollmentTerm[]>([])
const selectedUser = ref<User | null>(null)
const selectedTerm = ref<EnrollmentTerm | null>(null)

const correctedFromDate = ref<Date | null>(null)
const fromDate = ref<Date | null>(null)
const toDate = ref<Date | null>(null)
const runtimeConfig = useRuntimeConfig()
const store = useEpsilonStore()
onMounted(() => {
	api.filter
		.filterAccessibleStudentsList()
		.then((r) => {
			users.value = r.data
			selectedUser.value = users.value[0]
		})
		.catch((r) => store.addError(r))
})

// When the user is updated, we should request its terms
watch(selectedUser, () => {
	if (!selectedUser?.value?._id) {
		return
	}
	terms.value = []
	selectedTerm.value = null
	emit("userChange", selectedUser.value)

	api.filter
		.filterParticipatedTermsList({
			studentId: selectedUser.value._id,
		})
		.then((r) => {
			terms.value = r.data
			selectedTerm.value = terms.value[0]
		})
		.catch((r) => store.addError(r))
})

watch(selectedTerm, () => {
	const selectedTermUnwrapped = selectedTerm.value
	if (!selectedTermUnwrapped?.startAt || !selectedTermUnwrapped.endAt) {
		return
	}

	const termsUnwrapped = terms.value
	correctedFromDate.value = new Date(
		termsUnwrapped[termsUnwrapped.length - 1]?.startAt!
	)

	toDate.value = new Date(selectedTermUnwrapped?.endAt)
	fromDate.value = new Date(selectedTermUnwrapped?.startAt)
})

watch([correctedFromDate, toDate], () => {
	emit("rangeChange", {
		startCorrected: correctedFromDate.value,
		start: fromDate.value,
		end: toDate.value,
	})
})
</script>

<style lang="scss">
.top-navigation {
	padding: 1rem 1.5rem;
	background-color: #11284c;
	width: 100%;
	border-radius: 0.5rem;

	.v-autocomplete .v-field {
		padding: 3px 6px;
	}

	.v-autocomplete {
		background-color: #ffffff;
		border-radius: 6px;
	}
	.v-autocomplete .v-input__details {
		display: none;
	}

	&-logo {
		height: 3rem;
		object-fit: contain;
	}

	a {
		position: relative;
	}

	&-beta {
		color: #ffffff;
		width: max-content;
		text-align: center;
		padding: 2px 3px;
		background-color: #848da4;
		border-radius: 6px;
		position: absolute;
		right: -20px;
		top: -40px;
	}
}
</style>
