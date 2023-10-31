<template>
    <TopNavigation
        v-on:user-change="handleUserChange"
        v-on:term-change="handleTermChange"
    />
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
                    <PerformanceDashboard :submissions="filteredSubmissions" />
                </TabPanel>
            </TabPanels>
        </main>
    </TabGroup>
</template>

<script lang="ts" setup>
import { Tab, TabGroup, TabList, TabPanel, TabPanels } from "@headlessui/vue"
import TopNavigation from "~/components/TopNavigation.vue"
import {
    type EnrollmentTerm,
    type LearningDomainSubmission,
    type User,
} from "~/api.generated"

const { readCallback, validateCallback } = useLti()

const { data } = await useAsyncData(async (ctx) => {
    if (!process.server) {
        return
    }

    const event = useRequestEvent()
    return await readCallback(event)
})

if (process.client && data.value?.idToken) {
    const callback = data.value
    const validation = validateCallback(callback)

    if (validation) {
       useState("id_token", () => callback?.idToken)
    }
}

const api = useApi()

const submissions = ref<LearningDomainSubmission[]>([])
const filterRange = ref<{ start: number, end: number } | null>(null)

const filteredSubmissions = computed(() => {
    const unwrappedFilterRange = filterRange.value

    if (!unwrappedFilterRange) {
        return submissions.value
    }

    return submissions.value.filter(submission => {
        const submittedAt = Date.parse(submission.submittedAt!)

        return submittedAt <= unwrappedFilterRange.end
    })
})

const handleUserChange = async (user: User) => {
    if (user._id === null) {
        return
    }

    const outcomesResponse = await api?.learning.outcomesList({
        studentId: user._id,
    })

    submissions.value = outcomesResponse.data
}

const handleTermChange = async (term: EnrollmentTerm) => {
    filterRange.value = {
        start: Date.parse(term.start_at!),
        end: Date.parse(term.end_at!),
    }
}
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
