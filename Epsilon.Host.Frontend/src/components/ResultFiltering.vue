<template>
	<slot :terms="terms"></slot>
	<v-col cols="12" md="3">
		<v-autocomplete
			v-model="selectedUser"
			label="Students"
			:items="users"
			density="compact"
			:clearable="true"
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
			no-data-text
			@update:model-value="customDateSelection = false">
			<template #selection="{ item }">
				<span v-if="customDateSelection"> Custom </span>
				<span v-else>{{ item.title }}</span>
			</template>
			<template #prepend-item>
				<v-menu :close-on-content-click="false">
					<template #activator="{ props }">
						<v-list-item v-bind="props"> Custom </v-list-item>
					</template>
					<v-list>
						<v-list-item>
							<v-text-field
								label="From"
								:value="
									correctedFromDate
										?.toISOString()
										.slice(0, 10)
								"
								density="compact"
								type="date"
								@update:model-value="
									(s: string) => {
										customDateSelection = true
										correctedFromDate = new Date(s)
										fromDate = new Date(s)
									}
								"></v-text-field>
						</v-list-item>
						<v-list-item>
							<v-text-field
								label="Until"
								:value="toDate?.toISOString().slice(0, 10)"
								density="compact"
								type="date"
								@update:model-value="
									(s: string) => {
										customDateSelection = true
										toDate = new Date(s)
									}
								"></v-text-field>
						</v-list-item>
					</v-list>
				</v-menu>
			</template>
		</v-autocomplete>
	</v-col>
</template>

<script setup lang="ts">
import type { EnrollmentTerm, User } from "~/api.generated"

const emit = defineEmits(["userChange", "rangeChange"])
const api = useApi()

const users = ref<User[]>([])
const terms = ref<EnrollmentTerm[]>([])
const selectedUser = ref<User | null>(null)
const selectedTerm = ref<EnrollmentTerm | null>(null)

const correctedFromDate = ref<Date | null>(null)
const fromDate = ref<Date | null>(null)
const toDate = ref<Date | null>(null)
const customDateSelection = ref<boolean>(false)
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

watch([correctedFromDate, toDate, fromDate], () => {
	emit("rangeChange", {
		startCorrected: correctedFromDate.value,
		start: fromDate.value,
		end: toDate.value,
	})
})
</script>

<style scoped lang="scss"></style>
