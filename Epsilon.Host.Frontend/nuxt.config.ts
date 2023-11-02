import { readFileSync } from "fs"

const key = process.env.NUXT_SSL_KEY_PATH
const certificate = process.env.NUXT_SSL_CERT_PATH

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
	ssr: true,
	pages: true,
	runtimeConfig: {
		redirectUri: process.env.NUXT_REDIRECT_URI || "https://localhost:3000",
		overrideIdentityToken: process.env.NUXT_OVERRIDE_IDENTITY_TOKEN,
		public: {
			apiEndpoint: process.env.NUXT_API_ENDPOINT,
		},
	},
})
