// Create a new store instance.
import { createStore } from "vuex"
import {
    EnrollmentTerm,
    LearningDomain,
    LearningDomainOutcome,
    LearningDomainSubmission,
    User,
} from "@/api"

interface StoreState {
    domain: LearningDomain | null
    submissions: LearningDomainSubmission[]
    filterdSubmissions: LearningDomainSubmission[]
    outcomes: LearningDomainOutcome[]
    userTerms: EnrollmentTerm[]
    currentTerm: EnrollmentTerm | null
    users: User[]
    currentUser: User | null
}

const store = createStore({
    state(): StoreState {
        return {
            currentTerm: null,
            currentUser: null,
            domain: null,
            filterdSubmissions: [],
            outcomes: [],
            submissions: [],
            userTerms: [],
            users: [],
        }
    },
    mutations: {
        setCurrentTerm(state, currentTerm: EnrollmentTerm) {
            state.currentTerm = currentTerm
        },
        correctCurrentTerm(state) {
            if (state.currentTerm) {
                const index = state.userTerms?.indexOf(
                    state.currentTerm
                ) as number
                state.currentTerm.end_at =
                    index > 0
                        ? state.userTerms?.at(index - 1)?.start_at
                        : state.userTerms?.at(index)?.end_at
            }
        },
        setCurrentUser(state, currentUser: User) {
            state.currentUser = currentUser
        },
        setDomain(state, domain: LearningDomain) {
            state.domain = domain
        },
        filterSubmissions(state) {
            state.filterdSubmissions = state.submissions.filter(
                (submission) =>
                    submission.submittedAt > state.currentTerm.start_at &&
                    submission.submittedAt < state.currentTerm.end_at
            )
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
