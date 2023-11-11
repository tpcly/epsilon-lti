<template>
	<div class="top-navigation">
		<img alt="logo" class="top-navigation-logo" src="../assets/logo.png" />
		<Row>
			<Col :cols="6">
				<SearchBox
					v-model="selectedUser"
					:items="users"
					:limit="5"
					placeholder="Student" />
			</Col>
			<Col :cols="3">
				<TermDateFilter
					:from-date="fromDate"
					:to-date="toDate"
					placeholder="Term"
					@update:from-date="fromDate = $event"
					@update:to-date="toDate = $event" />
			</Col>
			<Col :cols="3">
				<SearchBox
					v-model="selectedTerm"
					:items="terms"
					:limit="10"
					placeholder="Term" />
			</Col>
		</Row>
	</div>
</template>

<script lang="ts" setup>
import SearchBox from "~/components/SearchBox.vue"
import TermDateFilter from "@/components/TermDateFilter.vue"
import Row from "~/components/LayoutRow.vue"
import Col from "~/components/LayoutCol.vue"
import { type EnrollmentTerm, type User } from "~/api.generated"
import { ref } from "vue"

const emit = defineEmits(["userChange", "rangeChange"])
const api = useApi()

const users = ref<User[]>([])
const terms = ref<EnrollmentTerm[]>([])
const selectedUser = ref<User | null>(null)
const selectedTerm = ref<EnrollmentTerm | null>(null)

const fromDate = ref<Date | null>(null)
const toDate = ref<Date | null>(null)

onMounted(async () => {
	const response = await api.filter.accessibleStudentsList()

	users.value = response.data
	selectedUser.value = users.value[0]
})

// When the user is updated, we should request its terms
watch(selectedUser, async () => {
	if (!selectedUser?.value?._id) {
		return
	}

	emit("userChange", selectedUser.value)

	terms.value = []

	const response = await api.filter.participatedTermsList({
		studentId: selectedUser.value._id,
	})

	terms.value = response.data
	selectedTerm.value = terms.value[0]
})

watch(selectedTerm, () => {
	const selectedTermUnwrapped = selectedTerm.value
	if (!selectedTermUnwrapped?.start_at || !selectedTermUnwrapped.end_at) {
		return
	}

	const termsUnwrapped = terms.value

	fromDate.value = new Date(
		termsUnwrapped[termsUnwrapped.length - 1]?.start_at!
	)

	toDate.value = new Date(selectedTermUnwrapped?.end_at)
})

watch([fromDate, toDate], () => {
	emit("rangeChange", {
		start: fromDate.value,
		end: toDate.value,
	})
})
</script>

<style lang="scss" scoped>
.top-navigation {
	display: flex;
	justify-content: space-between;
	align-items: center;
	padding: 2rem 3rem;
	background-color: #f2f3f8;
	width: 100%;
	border-radius: 0.5rem;

	&-logo {
		height: 5rem;
		object-fit: contain;
	}
}
</style>
