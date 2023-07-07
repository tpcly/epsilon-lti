import { createApp } from "vue"
import router from "./router"
import App from "./App.vue"

import { Api } from "@/api"

const app = createApp(App)
app.provide(
    "api",
    new Api({
        baseUrl: import.meta.env.VITE_EPSILON_API_ENDPOINT ?? "api",
    })
)

app.use(router).mount("#app")
