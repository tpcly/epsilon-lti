<template>
	<table class="learning-domain-values">
		<tr
			v-for="type of types"
			:key="type"
			class="learning-domain-values-row"
		>
			<td
				class="learning-domain-values-value-color"
				:style="{ backgroundColor: '#' + type.hexColor }"
			/>
			<th class="learning-domain-values-value-text">
				{{ type.name }}
			</th>
		</tr>
	</table>
</template>

<script lang="ts" setup>
import {
	type LearningDomain,
} from "~/api.generated"

const props = defineProps<{
    domain: LearningDomain
}>()

// TODO: Centralize this ugly sorting compare function, maybe possible with lodash?
const types = computed(() => {
	const types = props.domain.valuesSet.types
	return types.sort((a, b) => (a.order ?? 0) - (b.order ?? 0))
})
</script>

<style lang="scss">
.learning-domain-values {
    display: flex;
    flex-direction: column;
    width: fit-content;
    height: fit-content;
    padding: 2rem;

    &-row {
        display: grid;
        grid-template-columns: repeat(2, 5rem);
        padding: 0.5rem;
    }

    &-value {
        &-text {
            font-weight: 400;
        }

        &-color {
            width: 3rem;
        }
    }
}
</style>
