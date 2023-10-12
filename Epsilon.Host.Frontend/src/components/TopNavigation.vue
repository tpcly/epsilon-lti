<template>
	<div class="header">
		<img alt="logo" class="header-logo" src="../assets/logo.png" />
		<Row>
			<Col :cols="9">
				<!--TODO Component is used to selected users for upcoming feature-->
				<!--                <div class="d-flex">-->
				<!--                    <SearchBox-->
				<!--                        v-model="store.state.currentUser"-->
				<!--                        :items="store.state.users"-->
				<!--                        :limit="5"-->
				<!--                        placeholder="Student" />-->
				<!--                </div>-->
			</Col>
		</Row>
		<Row class="header-row2">
			<Col id="dateRangeCol" xs="12" sm="6" md="6" lg="6">
				<TermDateFilter :items="store.state.currentTerm" />
			</Col>
			<Col xs="12" sm="6" md="6" lg="6">
				<SearchBox
					v-model="selectedTerm"
					:items="store.state.userTerms"
					placeholder="Term" />
			</Col>
		</Row>
	</div>
</template>

<script lang="ts" setup>
import SearchBox from "@/components/SearchBox.vue"
import TermDateFilter from "@/components/TermDateFilter.vue"
import Row from "@/components/LayoutRow.vue"
import Col from "@/components/LayoutCol.vue"

import { inject, ref, Ref, watch } from "vue"
import { Api, EnrollmentTerm, HttpResponse } from "@/api.generated"
import { useStore } from "vuex"

const api = inject<Api<unknown>>("api")
const store = useStore()
const selectedTerm: Ref<EnrollmentTerm | undefined> = ref(undefined)

watch(selectedTerm, () => {
	store.commit("setCurrentTerm", selectedTerm.value)
	store.commit("filterSubmissions")
})

watch(store.state.currentUser, () => {
	if (!store.state.currentUser?._id) {
		return
	}

	// Reset current term list
	store.commit("setUserTerms", [])

	api?.filter
		.participatedTermsList({
			studentId: store.state.currentUser?._id,
		})
		.then((r: HttpResponse<EnrollmentTerm[]>) => {
			store.commit("setUserTerms", r.data)
			selectedTerm.value = store.state.currentTerm
			store.commit("setCurrentTerm", store.state.userTerms[0])
		})
})

api?.filter
	.participatedTermsList({
		studentId: import.meta.env.VITE_USER_ID,
	})
	.then((r: HttpResponse<EnrollmentTerm[]>) => {
		store.commit("setUserTerms", r.data)
		store.commit("setCurrentTerm", store.state.userTerms[0])
		selectedTerm.value = store.state.currentTerm
	})
</script>

<style lang="scss" scoped>
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

	&-row2 {
		margin-right: -120px;
	}
}
#dateRangeCol {
	margin-right: 30px;
}
</style>
