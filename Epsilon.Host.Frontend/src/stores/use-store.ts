import type { EnrollmentTerm, User } from "~/api.generated"
import { defineStore } from "pinia"
import type { TermRange } from "~/composables/use-services"

export const useEpsilonStore = defineStore("Epsilon", {
	state: () => {
		return {
			errors: [] as string[],
			terms: [] as EnrollmentTerm[],
			selectedTerm: {} as EnrollmentTerm | null,
			selectedTermRange: {} as TermRange | null,
			users: [] as User[],
			selectedUser: {} as User | null,
		}
	},
	actions: {
		addError(error: any) {
			this.errors.push(error)
		},
		setTerms(terms: EnrollmentTerm[]) {
			this.terms = terms
		},
		setSelectedTerm(term: EnrollmentTerm | null) {
			this.selectedTerm = term
		},
		setSelectedTermRange(term: TermRange | null) {
			this.selectedTermRange = term
		},
		setUsers(users: User[]) {
			this.users = users
		},
		setSelectedUser(user: User | null) {
			this.selectedUser = user
		},
	},
})
