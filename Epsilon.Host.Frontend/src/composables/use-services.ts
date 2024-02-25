import type { User } from "~/api.generated"
import { useEpsilonStore } from "~/stores/use-store"
const api = useApi()

export interface TermRange {
	customSelection: boolean
	startCorrected: Date
	start: Date
	end: Date
}

const loadTerms = (user: User): void => {
	const store = useEpsilonStore()
	store.setTerms([])
	store.setSelectedTerm(null)
	api.filter
		.filterParticipatedTermsList({
			studentId: user._id ?? "",
		})
		.then((r) => {
			store.setTerms(r.data)
			store.setSelectedTerm(store.terms[0])
		})
		.catch((r) => store.addError(r))
}

const loadStudents = (): void => {
	const store = useEpsilonStore()
	api.filter
		.filterAccessibleStudentsList()
		.then((r) => {
			store.setUsers(r.data)
			store.setSelectedUser(store.users[0])
			loadTerms(store.selectedUser!)
		})
		.catch((r) => store.addError(r))
}

export const useServices = (): {
	loadTerms: (user: User) => void
	loadStudents: () => void
} => {
	return {
		loadTerms,
		loadStudents,
	}
}
