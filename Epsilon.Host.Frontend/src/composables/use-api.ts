import { Api } from "~/api.generated"

export const useApi = (): Api<unknown> => {
	const runtimeConfig = useRuntimeConfig()
	const token = useState("id_token")

	return new Api({
		baseUrl: runtimeConfig.public.apiEndpoint,
		baseApiParams: {
			headers: {
				Authorization: `Bearer ${token.value}`,
			},
		},
	})
}
