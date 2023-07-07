<template>
    <div class="searchbox">
        <Combobox
            :model-value="props.modelValue"
            @update:model-value="$emit('update:modelValue', $event)">
            <div class="searchbox-input">
                <ComboboxInput
                    :display-value="displayValue"
                    :placeholder="placeholder"
                    @change="query = $event.target.value" />
                <ComboboxButton class="searchbox-list-arrow">
                    <ChevronUpDownIcon aria-hidden="true" />
                </ComboboxButton>
            </div>
            <ComboboxOptions v-if="items.length > 0" class="searchbox-options">
                <div
                    v-if="filteredItems.length === 0 && query !== ''"
                    class="searchbox-options-item">
                    No results found
                </div>

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
</template>

<script lang="ts" setup>
import { computed, defineProps, ref } from "vue"
import {
    Combobox,
    ComboboxButton,
    ComboboxInput,
    ComboboxOptions,
    ComboboxOption,
} from "@headlessui/vue"
import { CheckIcon, ChevronUpDownIcon } from "@heroicons/vue/20/solid"

const props = defineProps<{
    items: Array<{ name: string }>
    modelValue: { name: string }
    placeholder: string
}>()

const query = ref("")

defineEmits(["update:modelValue"])

const filteredItems = computed(() => {
    if (query.value === "") {
        return props.items
    }

    return props.items.filter((item) =>
        item.name
            .toLowerCase()
            .replace(/\s+/g, "")
            .includes(query.value.toLowerCase().replace(/\s+/g, ""))
    )
})

function displayValue(item: { name: string }): string {
    if (item) {
        return item.name
    }

    return ""
}
</script>

<style scoped lang="scss">
.searchbox {
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
        width: 100%;
        padding: 0.75rem;

        input {
            width: 100%;
            border: none;
            outline: none;
            font-size: 1rem;
            border-radius: 6px;
        }
    }

    &-options {
        position: absolute;
        background-color: #fff;
        width: 100%;
        list-style-type: none;
        border-radius: 6px;
        border: 1px solid #d8d8d8;
        text-align: left;
        z-index: 40;

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
</style>
