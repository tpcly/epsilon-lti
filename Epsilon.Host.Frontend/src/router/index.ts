import { createRouter, createWebHistory } from "vue-router"
import DashboardView from "@/views/DashboardView.vue"

const routes = [
	{
		path: "/",
		name: "Dashboard",
		component: DashboardView,
	},
]
const router = createRouter({
	history: createWebHistory(import.meta.env.BASE_URL),
	routes,
})
export default router
