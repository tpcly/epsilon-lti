import { readFileSync } from "fs"
import pkg from "./package.json"
import vuetify, { transformAssetUrls } from "vite-plugin-vuetify"
const key = process.env.NUXT_SSL_KEY_PATH
const certificate = process.env.NUXT_SSL_CRT_PATH

export default defineNuxtConfig({
 srcDir: "src",
 devtools: { enabled: true },

 typescript: {
					strict: true,
	},

 devServer: {
					https: {
									key: key ? readFileSync(key).toString() : undefined,
									cert: certificate
													? readFileSync(certificate).toString()
													: undefined,
					},
	},

 build: {
					transpile: [/vue/],
	},
 modules: [
					(_options, nuxt): void => {
									nuxt.hooks.hook("vite:extendConfig", (config) => {
													// @ts-expect-error
													config.plugins.push(vuetify({ autoImport: true }))
									})
					},
	],

 ssr: true,
 pages: true,

 runtimeConfig: {
					redirectUri: process.env.NUXT_REDIRECT_URI || "https://localhost:3000",
					overrideIdentityToken: process.env.NUXT_OVERRIDE_IDENTITY_TOKEN,
					public: {
									apiEndpoint: process.env.NUXT_API_ENDPOINT,
									clientVersion: pkg.version,
					},
	},

 vite: {
					vue: {
									template: {
													transformAssetUrls,
									},
					},
	},

 compatibilityDate: "2024-07-23",
})