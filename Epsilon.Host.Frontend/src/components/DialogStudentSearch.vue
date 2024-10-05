<template>
	<v-dialog :open class="wrapped-dialog" :model-value="true">
		<template #default="{ isActive }">
			<v-row>
				<v-col cols="12" :md="store.featureFlags.eduBadge ? 6 : 12">
					<v-card>
						<v-toolbar>
							<v-toolbar-title>Student selection</v-toolbar-title>
						</v-toolbar>
						<v-card-text>
							<StudentSelection></StudentSelection>
						</v-card-text>
						<v-card-actions>
							<v-spacer></v-spacer>
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
				</v-col>
				<v-col v-if="store.featureFlags.eduBadge" cols="12" md="6">
					<v-card>
						<v-toolbar>
							<v-toolbar-title>
								Edu-badge generation
							</v-toolbar-title>
						</v-toolbar>
						<v-card-actions>
							<v-spacer></v-spacer>
							<EdubadgeGenerationButton />
						</v-card-actions>
					</v-card>
				</v-col>
			</v-row>
		</template>
	</v-dialog>
</template>

<script setup lang="ts">
import StudentSelection from "~/components/filtering/StudentSelection.vue"
import { useServices } from "~/composables/use-services"
import { useEpsilonStore } from "~/stores/use-store"
import EdubadgeGenerationButton from "~/components/competence/EdubadgeGenerationButton.vue"
const store = useEpsilonStore()
</script>

<style scoped lang="scss">
.v-toolbar {
	background-color: #11284c;
	color: #ffffff;
}
</style>
