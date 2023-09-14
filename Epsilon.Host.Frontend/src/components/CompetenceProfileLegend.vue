<template>
    <table class="competence-profile-legend">
        <tr
            v-for="level of store.state.domain.valuesSet?.types?.sort((a,b) => a.shortName - b.shortName) as LearningDomainType[]"
            :key="level"
            class="competence-profile-legend-row">
            <td
                class="kpi-color"
                :style="{ backgroundColor: '#' + level.hexColor }" />
            <th class="kpi-text">{{ level.name }}</th>
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

.kpi-text {
    font-weight: 400;
}

.kpi-color {
    width: 3rem;
}

.competence-profile-legend-row {
    display: grid;
    grid-template-columns: repeat(2, 5rem);
    padding: 0.5rem;
}
</style>
