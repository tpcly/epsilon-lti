import posthog from "posthog-js"

export class Posthog {
	static init(): void {
		posthog.init("phc_wdX1JI1sEoh51qmTetv3MIvy5Sa8QywWgCrxTKzcCDI", {
			api_host: "https://eu.posthog.com",
			session_recording: {
				maskAllInputs: true,
				maskTextSelector: "*",
			},
		})
	}
}
