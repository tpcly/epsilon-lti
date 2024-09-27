import type { LearningDomain, User } from "~/api.generated"
import { useEpsilonStore } from "~/stores/use-store"
import * as domain from "node:domain"

export interface TermRange {
	customSelection: boolean
	startCorrected: Date
	start: Date
	end: Date
}

const loadTerms = (user: User): void => {
	const store = useEpsilonStore()
	const api = useApi()
	if (
		store.selectedUser?._id === null ||
		store.selectedUser?._id === undefined
	) {
		return
	}
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
	if (
		store.selectedUser?._id === null ||
		store.selectedUser?._id === undefined
	) {
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
	store.setUsedDomains(getUsedDomainsByUser())
	store.setLoadingSubmissions(false)
	filterSubmissions()
}

const getUsedDomainsByUser = (): string[] => {
	const store = useEpsilonStore()
	const uniqueDomainIds = new Set<string>()

	store.submissions.forEach((submission) => {
		submission.results?.forEach((result) => {
			const domainId = store.outcomes.find(
				(outcome) => outcome.id === result.outcome?.id
			)?.domain.id
			if (domainId) {
				uniqueDomainIds.add(domainId)
			}
		})
	})

	return uniqueDomainIds.size > 0
		? Array.from(uniqueDomainIds)
		: store.usedDomains
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

const getDomain = (hasColumnsRow: boolean): LearningDomain => {
	const store = useEpsilonStore()
	return store.domains.find(
		(l) =>
			store.usedDomains.includes(l.id!) &&
			(hasColumnsRow
				? l.columnsSet != undefined
				: l.columnsSet == undefined)
	)!
}

export const useServices = (): {
	loadTerms: (user: User) => void
	getDomain: (hasColumnsRow: boolean) => LearningDomain
	loadSubmissions: () => Promise<void>
	filterSubmissions: () => void
	loadStudents: () => void
	loadDomains: (domainNames: string[]) => void
} => {
	return {
		loadTerms,
		loadStudents,
		loadDomains,
		loadSubmissions,
		filterSubmissions,
		getDomain,
	}
}
