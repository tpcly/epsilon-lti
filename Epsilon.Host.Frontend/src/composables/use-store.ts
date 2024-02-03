import { defineStore } from "pinia"

export const useEpsilonStore = defineStore("Epsilon", {
	state: () => {
		return {
			errors: [] as string[],
		}
	},
	actions: {
		addError(error: any) {
			this.errors.push(error)
		},
	},
})
