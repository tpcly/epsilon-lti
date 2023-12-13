<template>
	<div class="top-navigation">
		<img
			alt="logo"
			class="top-navigation-logo"
			src="../assets/logo-white.png" />
		<Row class="search-boxes">
			<Col :cols="7">
				<SearchBox
					v-model="selectedUser"
					:items="users"
					placeholder="Student"
					:is-term-search="null" />
			</Col>
			<Col :cols="5">
				<SearchBox
					v-model="selectedTerm"
					:items="terms"
					placeholder="Term"
					:is-term-search="true" />
			</Col>
		</Row>
	</div>
</template>

<script lang="ts" setup>
import SearchBox from "~/components/SearchBox.vue"
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

const correctedFromDate = ref<Date | null>(null)
const fromDate = ref<Date | null>(null)
const toDate = ref<Date | null>(null)

onMounted(async () => {
	const response = await api.filter.filterAccessibleStudentsList()

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

	const response = await api.filter.filterParticipatedTermsList({
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

	correctedFromDate.value = new Date(
		termsUnwrapped[termsUnwrapped.length - 1]?.start_at!
	)

	toDate.value = new Date(selectedTermUnwrapped?.end_at)
	fromDate.value = new Date(selectedTermUnwrapped?.start_at)
})

watch([correctedFromDate, toDate], () => {
	emit("rangeChange", {
		startCorrected: correctedFromDate.value,
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
	background-color: #11284c;
	width: 100%;
	border-radius: 0.5rem;

	&-logo {
		height: 5rem;
		object-fit: contain;
	}
}

.search-boxes {
	margin-right: 10%;
}
</style>
