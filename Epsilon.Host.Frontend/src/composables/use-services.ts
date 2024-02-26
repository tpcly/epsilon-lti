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

const loadSubmissions = async (): Promise<void> => {
	const store = useEpsilonStore()
	const api = useApi()
	if (store.selectedUser?._id === null) {
		return
	}
	store.setLoadingSubmissions(true)

	const response = await api?.learning.learningOutcomesList({
		studentId: store.selectedUser!._id,
	})

	if (response.error) {
		store.addError(response.error)
	}
	store.setSubmissions(response.data)
	store.setLoadingSubmissions(false)
	filterSubmissions()
}

const filterSubmissions = (): void => {
	const store = useEpsilonStore()
	const unwrappedFilterRange = store.selectedTermRange
	if (!unwrappedFilterRange) {
		store.setFilteredSubmissions(store.submissions)
		return
	}

	store.setFilteredSubmissions(
		store.submissions.filter((submission) => {
			if (submission.criteria!.length > 0) {
				const submittedAt = new Date(submission.submittedAt!)

				return (
					submittedAt >= unwrappedFilterRange.startCorrected &&
					submittedAt <= unwrappedFilterRange.end
				)
			}
		})
	)
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
	loadSubmissions: () => void
	filterSubmissions: () => void
} => {
	return {
		loadTerms,
		loadStudents,
		loadDomains,
		loadSubmissions,
		filterSubmissions,
	}
}
