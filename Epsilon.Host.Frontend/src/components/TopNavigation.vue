<template>
    <div class="header">
        <img src="../assets/logo.png" alt="logo" class="header-logo" />
        <Row>
            <Col :cols="9">
                <div class="d-flex">
                    <img :src="avatarImage" class="avatar-image" alt="test" />
                    <SearchBox
                        v-model="selectedStudent"
                        :items="students"
                        placeholder="Student"
                        :limit="5" />
                </div>
            </Col>
            <Col :cols="3">
                <SearchBox
                    v-model="selectedTerm"
                    :items="terms"
                    placeholder="Term"
                    :limit="10" />
            </Col>
        </Row>
    </div>
</template>

<script setup lang="ts">
import DefaultAvatar from "@/assets/default_avatar.png"
import SearchBox from "@/components/SearchBox.vue"
import Row from "@/components/LayoutRow.vue"
import Col from "@/components/LayoutCol.vue"

import { computed, inject, onMounted, Ref, ref, watch } from "vue"
import { Api, EnrollmentTerm, HttpResponse, User } from "@/api"

const api = inject<Api<unknown>>("api")

const students: Ref<User[]> = ref([])
const selectedStudent: Ref<User | undefined> = ref(undefined)

const terms: Ref<EnrollmentTerm[]> = ref([])
const selectedTerm: Ref<EnrollmentTerm | undefined> = ref(undefined)

onMounted(() => {
    api?.filter.accessibleStudentsList().then((r: HttpResponse<User[]>) => {
        students.value = r.data
        selectedStudent.value = students.value[0]
    })
})

watch(selectedStudent, () => {
    if (!selectedStudent.value?._id) {
        return
    }

    // Reset current term list
    terms.value = []

    api?.filter
        .participatedTermsList({
            studentId: selectedStudent.value?._id,
        })
        .then((r: HttpResponse<EnrollmentTerm[]>) => {
            terms.value = r.data
            selectedTerm.value = terms.value[0]
        })
})

const avatarImage = computed(() => {
    if (!selectedStudent.value?.avatarUrl) {
        return DefaultAvatar
    }

    return selectedStudent.value?.avatarUrl
})
</script>

<style scoped lang="scss">
.header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 2rem 3rem;
    background-color: #f2f3f8;
    width: 100%;
    border-radius: 0.5rem;

    &-logo {
        height: 5rem;
        object-fit: contain;
    }
}

.avatar {
    &-image {
        min-width: 3rem;
        min-height: 3rem;
        max-width: 3rem;
        max-height: 3rem;
        aspect-ratio: 1/1;
        border: none;
        border-radius: 3rem;
        margin-right: 1rem;
        overflow: hidden;
        background-color: #fff;
    }
}
</style>
