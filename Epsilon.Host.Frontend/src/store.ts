// Create a new store instance.
import { createStore } from "vuex"
import {
	EnrollmentTerm,
	LearningDomain,
	LearningDomainOutcome,
	LearningDomainSubmission,
	User,
} from "@/api.generated"

interface StoreState {
	domain: LearningDomain | null
	personalDevelopment: LearningDomain | null
	submissions: LearningDomainSubmission[] | null
	filterdSubmissions: LearningDomainSubmission[] | null
	outcomes: LearningDomainOutcome[] | null
	userTerms: EnrollmentTerm[] | null
	currentTerm: EnrollmentTerm
	users: User[] | null
	currentUser: User
}

const store = createStore({
	state(): StoreState {
		return {
			currentTerm: {} as EnrollmentTerm,
			currentUser: {} as User,
			domain: null as unknown as LearningDomain,
			personalDevelopment: null as unknown as LearningDomain,
			filterdSubmissions: null as unknown as LearningDomainSubmission[],
			outcomes: null as unknown as LearningDomainOutcome[],
			submissions: null as unknown as LearningDomainSubmission[],
			userTerms: null as unknown as EnrollmentTerm[],
			users: null as unknown as User[],
		}
	},
	mutations: {
		setCurrentTerm(state, currentTerm: EnrollmentTerm) {
			state.currentTerm = currentTerm
		},
		setCurrentUser(state, currentUser: User) {
			state.currentUser = currentUser
		},
		setDomain(state, domain: LearningDomain) {
			state.domain = domain
		},
		setPersonalDevelopment(state, domain: LearningDomain) {
			state.personalDevelopment = domain
		},
		filterSubmissions(state) {
			state.filterdSubmissions = state.submissions
				?.filter(
					(submission) =>
						submission.submittedAt < state.currentTerm.end_at &&
						submission.criteria?.length > 0
				)
				.sort((a, b) =>
					a.assignment > b.assignment ? -1 : 1
				) as unknown as LearningDomainSubmission[]
		},
		setOutcomes(state, outcomes: LearningDomainOutcome[]) {
			state.outcomes = outcomes
		},
		setSubmissions(state, submissions: LearningDomainSubmission[]) {
			state.submissions = submissions
		},
		setUserTerms(state, terms: EnrollmentTerm[]) {
			state.userTerms = terms
		},
		setUsers(state, users: User[]) {
			state.users = users
		},
	},
})
export default store
