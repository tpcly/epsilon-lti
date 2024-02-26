<template>
	<v-dialog :open class="wrapped-dialog" :model-value="true">
		<template #default="{ isActive }">
			<v-card>
				<v-toolbar>
					<v-toolbar-title>Student search</v-toolbar-title>
				</v-toolbar>
				<v-card-text>
					<StudentSelection></StudentSelection>
				</v-card-text>
				<v-card-actions>
					<v-btn
						variant="text"
						@click="
							() => {
								isActive.value = false
								useServices().loadSubmissions()
								store.setStartUp(false)
							}
						">
						Search
					</v-btn>
				</v-card-actions>
			</v-card>
		</template>
	</v-dialog>
</template>

<script setup lang="ts">
import StudentSelection from "~/components/filtering/StudentSelection.vue"
import { useServices } from "~/composables/use-services"
import { useEpsilonStore } from "~/stores/use-store"
const store = useEpsilonStore()
</script>

<style scoped lang="scss">
.v-toolbar {
	background-color: #11284c;
	color: #ffffff;
}
</style>
