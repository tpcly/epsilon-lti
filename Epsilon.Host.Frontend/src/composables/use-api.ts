import { Api } from "~/api.generated"

export const useApi = () => {
    const runtimeConfig = useRuntimeConfig()
    const token = useState<string>("id_token").value

    return new Api({
        baseUrl: runtimeConfig.public.apiEndpoint,
        baseApiParams: {
            headers: {
                "Authorization": `Bearer ${token}`,
            },
        },
    })
}