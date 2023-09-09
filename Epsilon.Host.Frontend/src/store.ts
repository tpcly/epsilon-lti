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
            state.filterdSubmissions = state.submissions
                ?.filter(
                    (submission) =>
                        submission.submittedAt > state.currentTerm.start_at &&
                        submission.submittedAt < state.currentTerm.end_at
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
