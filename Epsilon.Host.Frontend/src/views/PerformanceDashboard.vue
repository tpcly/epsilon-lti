<template>
    <div>Performance dashboard</div>
    <CompetenceGraph v-if="store.state.submissions"></CompetenceGraph>
    <PersonalDevelopmentGraph
        v-if="store.state.submissions"></PersonalDevelopmentGraph>
</template>

<script lang="ts" setup>
import CompetenceGraph from "@/components/Performance/CompetenceGraph.vue"
import store from "@/store"
import PersonalDevelopmentGraph from "@/components/Performance/PersonalDevelopmentGraph.vue"
</script>
