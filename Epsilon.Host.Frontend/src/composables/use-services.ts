import type { User } from "~/api.generated"
import { useEpsilonStore } from "~/stores/use-store"

export interface TermRange {
	customSelection: boolean
	startCorrected: Date
	start: Date
	end: Date
}

const loadTerms = (user: User): void => {
	const store = useEpsilonStore()
	const api = useApi()
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
	const api = useApi()

	api.filter
		.filterAccessibleStudentsList()
		.then((r) => {
			store.setUsers(r.data)
			store.setSelectedUser(store.users[0])
			loadTerms(store.selectedUser!)
		})
		.catch((r) => store.addError(r))
}

const loadDomains = (domainNames: string[]): void => {
	const store = useEpsilonStore()
	const api = useApi()

	api.learning
		.learningDomainOutcomesList()
		.then((r) => store.setOutcomes(r.data))
		.catch((r) => store.addError(r))
	domainNames.map(function (domainName) {
		api.learning
			.learningDomainDetail(domainName)
			.then((r) => {
				const l = store.domains
				l.push(r.data)
				store.setDomains(l)
			})
			.catch((r) => store.addError(r))
	})
}

export const useServices = (): {
	loadTerms: (user: User) => void
	loadStudents: () => void
	loadDomains: (domainName: string[]) => void
} => {
	return {
		loadTerms,
		loadStudents,
		loadDomains,
	}
}
