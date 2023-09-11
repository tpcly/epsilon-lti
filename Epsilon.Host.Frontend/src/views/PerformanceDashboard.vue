<template>
    <div>Performance dashboard</div>
    <CompetenceGraph v-if="store.state.submissions"></CompetenceGraph>
</template>

<script lang="ts" setup>
import CompetenceGraph from "@/components/Performance/CompetenceGraph.vue"
import store from "@/store"
</script>
