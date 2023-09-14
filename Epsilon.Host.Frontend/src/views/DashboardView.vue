<template>
    <Header />
    <TabGroup as="template">
        <div class="toolbar mb-lg mt-lg">
            <div class="toolbar-slider">
                <TabList>
                    <Tab class="toolbar-slider-item">
                        Performance dashboard
                    </Tab>
                </TabList>
            </div>
        </div>
        <hr class="divider mb-lg" />
        <main>
            <TabPanels>
                <TabPanel>
                    <PerformanceDashboard />
                </TabPanel>
            </TabPanels>
        </main>
    </TabGroup>
</template>
<script lang="ts" setup>
import { Tab, TabGroup, TabList, TabPanel, TabPanels } from "@headlessui/vue"
import PerformanceDashboard from "@/views/PerformanceDashboard.vue"
import Header from "@/components/TopNavigation.vue"
import { Api, HttpResponse, User } from "@/api"
import { inject, onMounted } from "vue"
import { useStore } from "vuex"

const api = inject<Api<unknown>>("api")
const store = useStore()

onMounted(() => {
    api?.filter.accessibleStudentsList().then((r: HttpResponse<User[]>) => {
        store.commit("setUsers", r.data)
        store.commit(
            "setCurrentUser",
            store.state.users.find(
                (u: User) => u._id === import.meta.env.VITE_USER_ID
            )
        )
    })
    api?.learning
        .domainDetail("hbo-i-2018")
        .then((r) => store.commit("setDomain", r.data))

    api?.learning
        .domainDetail("pd-2020-bsc")
        .then((r) => store.commit("setPersonalDevelopment", r.data))

    api?.learning
        .outcomesList({
            studentId: import.meta.env.VITE_USER_ID ?? "00000",
        })
        .then((r) => {
            store.commit("setSubmissions", r.data)
            store.commit("filterSubmissions")
        })

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
