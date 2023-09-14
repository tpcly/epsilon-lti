<template>
    <LayoutRow v-if="store.state.filterdSubmissions">
        <layout-col cols="8">
            <CompetenceProfile
                :submissions="
                    store.state.filterdSubmissions
                "></CompetenceProfile>
        </layout-col>
        <layout-col cols="4"
            ><CompetenceProfileLegend></CompetenceProfileLegend>
        </layout-col>
        <layout-col cols="8">
            <CompetenceGraph></CompetenceGraph>
        </layout-col>
        <layout-col cols="4">
            <PersonalDevelopmentGraph></PersonalDevelopmentGraph>
        </layout-col>
    </LayoutRow>
</template>

<script lang="ts" setup>
import CompetenceGraph from "@/components/Performance/CompetenceGraph.vue"
import store from "@/store"
import PersonalDevelopmentGraph from "@/components/Performance/PersonalDevelopmentGraph.vue"
import CompetenceProfileLegend from "@/components/CompetenceProfileLegend.vue"
import CompetenceProfile from "@/components/Competence/CompetenceProfile.vue"
import LayoutRow from "@/components/LayoutRow.vue"
import LayoutCol from "@/components/LayoutCol.vue"
</script>

<style scoped></style>
