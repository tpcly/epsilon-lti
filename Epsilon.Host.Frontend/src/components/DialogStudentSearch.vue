<template>
	<v-dialog
		:open
		min-width="75%"
		min-height="75%"
		class="wrapped-dialog"
		:model-value="true">
		<template #default="{ isActive }">
			<v-card>
				<v-toolbar>
					<v-toolbar-title>Student search</v-toolbar-title>
				</v-toolbar>
				<v-card-text>
					<v-row>
						<v-spacer></v-spacer>
						<ResultFiltering></ResultFiltering>
						<v-col>
							<v-btn variant="text"> Search </v-btn>
						</v-col>
						<v-spacer></v-spacer>
					</v-row>
				</v-card-text>
			</v-card>
		</template>
	</v-dialog>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
.v-toolbar {
	background-color: #11284c;
	color: #ffffff;
}
</style>
