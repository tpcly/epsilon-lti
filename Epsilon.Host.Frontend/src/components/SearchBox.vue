<template>
	<div class="searchbox">
		<Combobox @update:model-value="$emit('update:modelValue', $event)">
			<div class="searchbox-input">
				<ComboboxInput
					:display-value="displayValue"
					:placeholder="placeholder"
					@change="query = $event.target.value" />
				<ComboboxButton class="searchbox-list-arrow">
					<ChevronUpDownIcon aria-hidden="true" />
				</ComboboxButton>
			</div>
			<ComboboxOptions v-if="items?.length > 0" class="searchbox-options">
				<div
					v-if="filteredItems?.length === 0 && query !== ''"
					class="searchbox-options-item">
					No results found
				</div>
				<li
					class="searchbox-options-item"
					:class="{ 'custom-click-color': customClick }"
					@click="handleCustomClick">
					Custom ﹥
				</li>
				<ComboboxOption
					v-for="(item, id) in filteredItems"
					:key="id"
					v-slot="{ selected, active }"
					as="template"
					:value="item"
					class="searchbox-options-item">
					<li :class="{ 'searchbox-options-item-active': active }">
						{{ item.name }}
						<CheckIcon
							v-if="selected"
							class="searchbox-options-item-select-icon"
							aria-hidden="true" />
					</li>
				</ComboboxOption>
			</ComboboxOptions>
		</Combobox>
	</div>
	<div v-if="customClick" class="custom-box">
		<div class="date-input">
			<label for="startDate">Start date:</label>
			<input
				id="startDate"
				:value="startDate"
				type="date"
				@input="updateStartDate" />
		</div>
		<div class="date-input">
			<label for="endDate">End date:</label>
			<input
				id="endDate"
				:value="endDate"
				type="date"
				@input="updateEndDate" />
		</div>
	</div>
</template>

<script lang="ts" setup>
import { computed, defineProps, ref, watch } from "vue"
import {
	Combobox,
	ComboboxButton,
	ComboboxInput,
	ComboboxOptions,
	ComboboxOption,
} from "@headlessui/vue"
import { CheckIcon, ChevronUpDownIcon } from "@heroicons/vue/20/solid"
import { EnrollmentTerm } from "@/api.generated"

const props = defineProps<{
	items: Array<{ name?: string | null }> | null
	modelValue: EnrollmentTerm | undefined
	placeholder?: string
	limit: number
}>()

const query = ref("")
const customClick = ref(false)
const startDate = ref<string | undefined>(undefined)
const endDate = ref<string | undefined>(undefined)
const termName = ref<string | undefined>(undefined)
let datesAdjusted = false

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
		datesAdjusted = false
	}
)

function displayValue(item: { name: string }): string {
	if (datesAdjusted) {
		termName.value = startDate.value + " - " + endDate.value
	}
	if (item && termName.value) {
		return termName.value
	}

	return ""
}

function handleCustomClick(): void {
	customClick.value = !customClick.value
}
</script>

<style scoped lang="scss">
.searchbox {
	height: 45px;
	position: relative;
	background-color: #fff;
	border-radius: 7px;
	width: 52%;

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
		box-shadow: 0px 1px 2px rgba(0, 0, 0, 0.2);

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
	position: fixed;
	border: 1px solid #d8d8d8;
	background-color: #fff;
	border-radius: 7px;
	padding: 10px;
	width: 150px;
	margin-top: 4px;
	margin-left: 125px;
}

.custom-click-color {
	background-color: #f2f3f8;
}
</style>
