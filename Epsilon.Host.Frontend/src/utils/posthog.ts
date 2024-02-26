import posthog, { PostHog } from "posthog-js"
import { useEpsilonStore } from "~/stores/use-store"

export class Posthog {
	static init(): PostHog | void {
		const store = useEpsilonStore()
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

		p?.onFeatureFlags(function () {
			store.setFeatureFlags({
				eduBadge: p?.isFeatureEnabled("eduBadge") ?? false,
				teacherMode: p?.isFeatureEnabled("teacherMode") ?? false,
			})
		})
		return p
	}
}
