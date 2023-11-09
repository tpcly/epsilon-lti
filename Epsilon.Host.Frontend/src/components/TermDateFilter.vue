<template>
	<div class="term-date-filter">
		<div class="term-date-filter-refresh">↻</div>
		<div @click="toggleMenu">Date range</div>
		<div class="term-date-filter-menu" :class="{ open: open }">
			<div class="date-input">
				<label for="from-date">Start date</label>
				<input
					id="from-date"
					type="date"
					:value="formatDate(fromDate)"
					@change="updateFromDate" />
			</div>
			<div class="date-input">
				<label for="to-date">End date</label>
				<input
					id="to-date"
					type="date"
					:value="formatDate(toDate)"
					@change="updateToDate" />
			</div>
		</div>
	</div>
</template>

<script lang="ts" setup>
import { ref, defineProps } from "vue"

defineProps<{
	fromDate: Date | null
	toDate: Date | null
}>()

const open = ref(false)
const emit = defineEmits(["update:fromDate", "update:toDate"])

const formatDate = (date: Date | null): string => {
	if (!date) {
		return ""
	}

	return date.toISOString().substring(0, 10)
}

const updateFromDate = (event: Event): void => {
	console.log(new Date(event.target.value))
	emit("update:fromDate", new Date(event.target.value))
}

const updateToDate = (event: Event): void => {
	emit("update:toDate", new Date(event.target.value))
}

// const updateDates = (term: EnrollmentTerm | null): void => {
// 	if (!term) {
// 		return
// 	}
//
// 	fromDate.value = new Date(term.start_at!).toISOString().substring(0, 10)
// 	toDate.value = new Date(term.end_at!).toISOString().substring(0, 10)
// }
//
// const handleDateChange = (): void => {
// 	emit("rangeChange", {
// 		start: Date.parse(fromDate.value!),
// 		end: Date.parse(toDate.value!),
// 	})
// }

const toggleMenu = (): void => {
	open.value = !open.value
}
</script>

<style lang="scss" scoped>
.term-date-filter {
	position: relative;
	width: 100%;
	background-color: #fff;
	border-radius: 7px;
	height: 100%;
	display: flex;
	align-items: center;
	padding: 0 1rem;

	&-menu {
		position: absolute;
		top: 100%;
		left: 0;
		width: 100%;
		display: none;
		background-color: #fff;
		padding: 0.75rem;
		box-shadow: 0 1px 2px rgba(0, 0, 0, 0.2);
		border-radius: 6px;
		border: 1px solid #d8d8d8;

		&.open {
			display: block;
		}
	}

	&-refresh {
		font-size: 1.3rem;
		margin-right: 0.5rem;
		cursor: pointer;
	}
}

.date-input {
	margin-bottom: 10px;

	label {
		display: block;
		margin-bottom: 5px;
	}

	input {
		font-family: inherit;
		font-size: 1rem;
		font-weight: 400;
		border: none;
	}
}
</style>
