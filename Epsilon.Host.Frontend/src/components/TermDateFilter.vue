<template>
	<button class="refresh-button" @click="resetDates">↻</button>
	<div ref="dateRangeMenu" class="date-range-menu">
		<button @click="toggleMenu">Date range</button>
		<div class="menu-content" :class="{ 'menu-visible': menuVisible }">
			<div class="date-range-filter">
				<div class="date-input">
					<label for="startDate">Start date:</label>
					<input
						id="startDate"
						v-model="startDate"
						type="date"
						@input="filterDates" />
				</div>
				<div class="date-input">
					<label for="endDate">End date:</label>
					<input
						id="endDate"
						v-model="endDate"
						type="date"
						@input="filterDates" />
				</div>
			</div>
		</div>
	</div>
</template>

<script lang="ts" setup>
import { ref, onMounted, defineProps } from "vue"
import { useStore } from "vuex"
import { EnrollmentTerm } from "@/api.generated"
const store = useStore()

const props = defineProps<{
	items: EnrollmentTerm | null
	modelValue: { name: string }
	placeholder?: string
	limit: number
}>()

const startDate = ref<string | null>(null)
const endDate = ref<string | null>(null)
const menuVisible = ref(false)
const originalItem = ref<EnrollmentTerm | null>(null)
const hasSetInitialItems = ref(false)
const dateRangeMenuRef = ref<HTMLElement | null>(null)

const setInitialItems = (): void => {
	if (props.items !== null) {
		originalItem.value = props.items
	}
}

const filterDates = (): void => {
	if (
		props.items !== null &&
		startDate.value !== null &&
		endDate.value !== null
	) {
		const updatedTerm = {
			...props.items,
			start_at: startDate.value,
			end_at: endDate.value,
		}

		store.commit("setCurrentTerm", updatedTerm)
	}
}

const resetDates = (): void => {
	if (originalItem.value !== null) {
		store.commit("setCurrentTerm", originalItem)
	}
}

const toggleMenu = (): void => {
	menuVisible.value = !menuVisible.value
	if (menuVisible.value) {
		document.addEventListener("click", closeMenuOnClickOutside)
	} else {
		document.removeEventListener("click", closeMenuOnClickOutside)
	}
}

const closeMenuOnClickOutside = (event: MouseEvent): void => {
	if (
		menuVisible.value &&
		dateRangeMenuRef.value &&
		!dateRangeMenuRef.value.contains(event.target as Node)
	) {
		menuVisible.value = false
		document.removeEventListener("click", closeMenuOnClickOutside)
	}
}

onMounted(() => {
	if (props.items && !hasSetInitialItems.value) {
		setInitialItems()
		hasSetInitialItems.value = true
	}
})
</script>

<style scoped>
.date-range-menu {
	position: relative;
	display: inline-block;
}

.menu-content {
	position: absolute;
	top: 100%;
	left: 0;
	display: none;
	background-color: #fff;
	padding: 0.75rem;
	box-shadow: 0 1px 2px rgba(0, 0, 0, 0.2);
	border-radius: 6px;
	border: 1px solid #d8d8d8;
}

.menu-visible {
	display: block;
}

.date-range-menu button {
	border: none;
	cursor: pointer;
	height: 45px;
	background-color: #fff;
	border-radius: 7px;
	font-family: inherit;
	font-size: 1rem;
	font-weight: 400;
	width: 135%;
	vertical-align: middle;
}

.date-input {
	margin-bottom: 10px;
}

.date-input label {
	display: block;
	margin-bottom: 5px;
}

.date-input input[type="date"] {
	font-family: inherit;
	font-size: 1rem;
	font-weight: 400;
}

.refresh-button {
	border: none;
	font-family: inherit;
	font-size: 1.3rem;
	font-weight: 400;
	border-radius: 7px;
	height: 45px;
	background-color: #fff;
	margin-right: 5px;
	vertical-align: middle;
	cursor: pointer;
}
</style>
