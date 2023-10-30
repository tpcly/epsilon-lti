import { defineConfig, loadEnv, UserConfigExport } from "vite"
import vue from "@vitejs/plugin-vue"
import { resolve } from "path"
import { readFileSync } from "fs"
import { Posthog } from "./posthog"

export default ({ mode }: { mode: string }): UserConfigExport => {
	process.env = { ...process.env, ...loadEnv(mode, process.cwd()) }
	const certificate = process.env.VITE_SSL_CRT_FILE
	const key = process.env.VITE_SSL_KEY_FILE
	const nodeEnv = process.env.NODE_ENV
	let serverConfig = {}
	if (nodeEnv == "development") {
		serverConfig = {
			https: {
				cert: certificate ? readFileSync(certificate) : undefined,
				key: key ? readFileSync(key) : undefined,
			},
			port: 8000,
		}
	} else {
		Posthog.init()
		serverConfig = {
			http: {},
			port: 8000,
		}
	}

	return defineConfig({
		resolve: {
			alias: {
				"@": resolve(__dirname, "src"),
			},
		},
		server: serverConfig,
		plugins: [vue()],
	})
}
