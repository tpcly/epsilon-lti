<template>
    <Header v-if="store.state.users" />
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
        <main v-if="store.state.domain && store.state.currentTerm">
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
<script lang="ts" setup>
import { Tab, TabGroup, TabList, TabPanel, TabPanels } from "@headlessui/vue"
import PerformanceDashboard from "@/views/PerformanceDashboard.vue"
import CompetenceDocument from "@/views/CompetenceDocument.vue"
import Header from "@/components/TopNavigation.vue"
import { Api, HttpResponse, User } from "@/api"
import { inject, onMounted } from "vue"
import { useStore } from "vuex"

const api = inject<Api<unknown>>("api")
const store = useStore()

onMounted(() => {
    api?.filter.accessibleStudentsList().then((r: HttpResponse<User[]>) => {
        store.commit("setUsers", r.data)
        store.commit("setCurrentUser", store.state.users[0])
    })
    api?.learning
        .domainDetail("hbo-i-2018")
        .then((r) => store.commit("setDomain", r.data))

    api?.learning
        .domainDetail("pd-2020-bsc")
        .then((r) => store.commit("setPersonalDevelopment", r.data))

    api?.learning
        .outcomesList({
            studentId: "20592",
        })
        .then((r) => store.commit("setSubmissions", r.data))

    api?.learning
        .domainOutcomesDetail("wajdgawlhdawhdgawjkd")
        .then((r) => store.commit("setOutcomes", r.data))
})
</script>

<style lang="scss" scoped>
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
