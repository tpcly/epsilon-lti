import posthog, { PostHog } from "posthog-js"

export class Posthog {
	static init(): PostHog | void {
		const p = posthog.init(
			"phc_wdX1JI1sEoh51qmTetv3MIvy5Sa8QywWgCrxTKzcCDI",
			{
				api_host: "https://eu.posthog.com",
				session_recording: {
					maskAllInputs: true,
					maskTextSelector: "*",
				},
			}
		)
		p?.setPersonPropertiesForFlags({ $current_url: location.href })
		console.log(location.href)
		return p
	}
}
