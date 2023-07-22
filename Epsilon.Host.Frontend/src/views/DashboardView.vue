<template>
    <Header />
    <TabGroup as="template">
        <div class="toolbar mb-lg mt-lg">
            <div class="toolbar-slider">
                <TabList>
                    <Tab class="toolbar-slider-item">
                        Performance dashboard
                    </Tab>
                    <Tab class="toolbar-slider-item">Competence document</Tab>
                </TabList>
            </div>
        </div>
        <hr class="divider mb-lg" />
        <main>
            <TabPanels>
                <TabPanel>
                    <PerformanceDashboard />
                </TabPanel>
                <TabPanel>
                    <CompetenceDocument />
                </TabPanel>
            </TabPanels>
        </main>
    </TabGroup>
</template>
<script setup lang="ts">
import { TabGroup, TabList, Tab, TabPanels, TabPanel } from "@headlessui/vue"
import PerformanceDashboard from "@/views/PerformanceDashboard.vue"
import CompetenceDocument from "@/views/CompetenceDocument.vue"
import Header from "@/components/TopNavigation.vue"
</script>

<style scoped lang="scss">
.toolbar {
    display: flex;
    justify-content: space-between;

    &-slider {
        list-style: none;
        background-color: #f2f3f8;
        padding: 5px;
        border-radius: 8px;
        width: fit-content;
        display: flex;
        align-items: center;
        justify-content: center;

        &-item {
            border-radius: 5px;
            padding: 0.6em 1.2em;
            cursor: pointer;
            background-color: transparent;
            border: none;
            font-size: 1em;

            &:active,
            &:focus {
                outline: transparent;
            }

            &:hover {
                background-color: #d8d9dd;
            }

            &[data-headlessui-state="selected"] {
                background-color: white;
            }
        }
    }
}

.divider {
    border: 1px solid #f2f3f8;
}
</style>
