<template>
    <table class="competence-profile-legend">
        <tr
            v-for="level of store.state.domain.valuesSet.types.sort((a,b) => a.shortName - b.shortName) as LearningDomainType[]"
            :key="level"
            class="competence-profile-legend-row">
            <td
                class="value-color"
                :style="{ backgroundColor: '#' + level.hexColor }" />
            <th class="value-text">{{ level.name }}</th>
        </tr>
    </table>
</template>

<script lang="ts" setup>
import store from "@/store"
import { LearningDomainType } from "@/api"
</script>

<style>
.competence-profile-legend {
    display: flex;
    flex-direction: column;
    width: fit-content;
    height: fit-content;
    padding: 2rem;
}

.value-text {
    font-weight: 400;
}

.value-color {
    width: 3rem;
}

.competence-profile-legend-row {
    display: grid;
    grid-template-columns: repeat(2, 5rem);
    padding: 0.5rem;
}
</style>
