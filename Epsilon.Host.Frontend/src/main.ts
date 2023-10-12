import { createApp } from "vue"
import router from "./router"
import App from "./App.vue"

import { Api } from "@/api.generated"
import store from "@/store"

const app = createApp(App)
app.provide(
	"api",
	new Api({
		baseUrl: import.meta.env.VITE_EPSILON_API_ENDPOINT ?? "api",
	})
)

app.use(router).use(store).mount("#app")
