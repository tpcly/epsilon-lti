<template>
	<button class="refresh-button" @click="resetDates">↻</button>
	<div ref="dateRangeMenu" class="date-range-menu">
		<button @click="toggleMenu">Date range</button>
		<div class="menu-content" :class="{ 'menu-visible': menuVisible }">
			<div class="date-range-filter">
				<div class="date-input">
					<label for="startDate">Start date:</label>
					<input id="startDate" v-model="startDate" type="date" />
				</div>
				<div class="date-input">
					<label for="endDate">End date:</label>
					<input id="endDate" v-model="endDate" type="date" />
				</div>
			</div>
		</div>
	</div>
</template>

<script lang="ts" setup>
import { ref, defineProps, watch } from "vue"
import { EnrollmentTerm } from "@/api.generated"

const props = defineProps<{
	currentTerm: EnrollmentTerm | undefined
}>()

const startDate = ref<string | undefined>(undefined)
const endDate = ref<string | undefined>(undefined)
const menuVisible = ref(false)

watch(
	() => props.currentTerm,
	(selection) => {
		if (!selection?.start_at || !selection.end_at) {
			return
		}

		startDate.value = selection.start_at.split("T")[0].replace(/\//g, "-")
		endDate.value = selection.end_at.split("T")[0].replace(/\//g, "-")
	}
)

const resetDates = (): void => {
	if (!props.currentTerm?.start_at || !props.currentTerm.end_at) {
		return
	}

	startDate.value = props.currentTerm.start_at
		.split("T")[0]
		.replace(/\//g, "-")
	endDate.value = props.currentTerm.end_at.split("T")[0].replace(/\//g, "-")
}

const toggleMenu = (): void => {
	menuVisible.value = !menuVisible.value
}
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
