<template>
	<div
		ref="searchBox"
		class="search-box"
		:style="{ width: isTermSearch ? '122px' : '100%' }">
		<Combobox
			v-slot="{ open }"
			:model-value="modelValue"
			@update:model-value="emit('update:modelValue', $event)">
			<div class="search-box-input">
				<ComboboxInput
					:display-value="displayValue"
					:placeholder="placeholder"
					@change="query = $event.target.value" />
				<ComboboxButton
					class="search-box-list-arrow"
					@click="
						() => {
							open.value = !open.value
						}
					">
					<ChevronUpDownIcon aria-hidden="true" />
				</ComboboxButton>
			</div>
			<ComboboxOptions
				v-if="items?.length > 0"
				:static="isStatic"
				class="search-box-options">
				<div
					v-if="filteredItems?.length === 0 && query !== ''"
					class="search-box-options-item">
					No results found
				</div>
				<div v-if="isTermSearch">
					<li
						id="isTermSearchBox"
						class="search-box-options-item"
						:class="{ 'custom-click-color': customClick }"
						@click="handleCustomClick">
						Custom﹥
					</li>
				</div>
				<ComboboxOption
					v-for="(item, id) in filteredItems"
					:key="id"
					v-slot="{ selected, active }"
					as="template"
					:value="item"
					class="search-box-options-item"
					:style="{ width: !isTermSearch ? '215px' : '100%' }">
					<li
						:class="{
							'search-box-options-item-active': active,
						}">
						{{ item.name }}
						<CheckIcon
							v-if="selected"
							class="search-box-options-item-select-icon"
							aria-hidden="true" />
					</li>
				</ComboboxOption>
			</ComboboxOptions>
		</Combobox>
	</div>
	<div v-if="customClick" class="custom-box">
		<h5 class="custom-header">Adjust term dates</h5>
		<div class="date-input">
			<label id="dateLabel" for="startDate">Start date</label>
			<input
				id="startDate"
				:value="startDate"
				type="date"
				@input="updateStartDate" />
		</div>
		<div class="date-input">
			<label id="dateLabel" for="endDate">End date</label>
			<input
				id="endDate"
				:value="endDate"
				type="date"
				@input="updateEndDate" />
		</div>
	</div>
</template>

<script lang="ts" setup>
import {
	Combobox,
	ComboboxButton,
	ComboboxInput,
	ComboboxOptions,
	ComboboxOption,
} from "@headlessui/vue"
import { CheckIcon, ChevronUpDownIcon } from "@heroicons/vue/20/solid"
import type { EnrollmentTerm } from "~/api.generated"

const props = defineProps<{
	items: Array<{ name?: string | null }> | null
	modelValue: EnrollmentTerm | null
	placeholder?: string
	limit: number
	isTermSearch: boolean | null
}>()

const query = ref<string>("")
const startDate = ref<string | undefined>(undefined)
const endDate = ref<string | undefined>(undefined)
let datesAdjusted = false

// Reset the default open/close behaviour of the combobox
const isStatic = ref<boolean>(false)
const customClick = ref<boolean>(false)

// For when combobox input is date or term name
let dateName = false
const termName = ref<string | undefined>(undefined)

const emit = defineEmits(["update:modelValue"])

const filteredItems = computed(() => {
	if (props.items === null) {
		return null
	}

	if (query.value === "") {
		return props.items.slice(0, props.limit)
	}

	return props.items
		.filter((item) => {
			if (!item.name) {
				return false
			}

			return item.name
				.toLowerCase()
				.replace(/\s+/g, "")
				.includes(query.value.toLowerCase().replace(/\s+/g, ""))
		})
		.slice(0, props.limit)
})

// Display term name or term dates (custom)
function displayValue(term: EnrollmentTerm): string {
	if (datesAdjusted) {
		if (startDate.value && endDate.value) {
			dateName = true
			return startDate.value + " - " + endDate.value
		}
	}
	if (term && term.name) {
		return term.name
	}
	return ""
}

// Handle open/close of the custom term box
function handleCustomClick(): void {
	customClick.value = !customClick.value
	isStatic.value = !isStatic.value
}

const updateStartDate = (event: Event): void => {
	const target = event.target as HTMLInputElement
	startDate.value = target.value
	emit("update:modelValue", {
		...props.modelValue,
		start_at: startDate.value,
	})
	datesAdjusted = true
}

const updateEndDate = (event: Event): void => {
	const target = event.target as HTMLInputElement
	endDate.value = target.value
	emit("update:modelValue", {
		...props.modelValue,
		end_at: endDate.value,
	})
	datesAdjusted = true
}

// Watch for changes and fill in date boxes
watch(
	() => props.modelValue,
	(selection) => {
		if (!selection?.start_at || !selection.end_at) {
			return
		}
		startDate.value = selection.start_at.split("T")[0].replace(/\//g, "-")
		endDate.value = selection.end_at.split("T")[0].replace(/\//g, "-")
		if (!customClick.value) {
			termName.value = selection.name || undefined
		}
		if (dateName && datesAdjusted) {
			datesAdjusted = false
			dateName = false
		}
	}
)
</script>

<style scoped lang="scss">
.search-box {
	position: relative;
	width: 100%;
	background-color: #fff;
	border-radius: 7px;

	&-input {
		position: relative;
		display: flex;
		align-items: center;
		justify-content: space-between;
		border: none;
		font-weight: 400;
		text-align: left;
		padding: 0.75rem;
		width: 100%;

		input {
			border: none;
			outline: none;
			font-size: 1rem;
			border-radius: 6px;
			width: 100%;
			font-family: inherit;
		}
	}

	&-options {
		position: absolute;
		background-color: #fff;
		list-style-type: none;
		border-radius: 6px;
		border: 1px solid #d8d8d8;
		text-align: left;
		z-index: 40;
		box-shadow: 0 1px 2px rgba(0, 0, 0, 0.2);

		&-item {
			padding: 1rem 1.5rem;
			cursor: pointer;
			display: flex;
			align-items: center;
			justify-content: space-between;

			&-active {
				background-color: #f2f3f8;
			}

			&-select-icon {
				padding-left: 4px;
				width: 20px;
				height: 20px;
			}
		}
	}

	&-list-arrow {
		background-color: transparent;
		border: none;
		border-radius: 0;

		&:hover,
		&:focus,
		&:active {
			border: none;
			outline: none;
		}

		svg {
			height: 20px;
			max-height: 30px;
		}
	}
}

.custom-box {
	position: absolute;
	border: 1px solid #d8d8d8;
	background-color: #fff;
	border-radius: 7px;
	padding: 10px;
	width: 15%;
	margin-left: 10.8%;
}

.custom-click-color {
	background-color: #f2f3f8;
}

.date-input input[type="date"] {
	font-family: inherit;
	font-size: 1rem;
	font-weight: 400;
	width: 100%;
}

#dateLabel {
	color: #0f254a;
}

.date-input {
	margin-bottom: 10px;
	margin-top: 5px;
}

.custom-header {
	background-color: #f2f3f8;
	padding: 5px;
	text-align: center;
}
</style>
