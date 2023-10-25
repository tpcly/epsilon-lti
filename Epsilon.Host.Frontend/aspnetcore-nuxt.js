// This script configures the .env file with additional environment variables to configure HTTPS using the ASP.NET Core
// development certificate in the webpack development proxy.
import fs from "fs"
import path from "path"

const baseFolder =
	process.env.APPDATA !== undefined && process.env.APPDATA !== ""
		? `${process.env.APPDATA}/ASP.NET/https`
		: `${process.env.HOME}/.aspnet/https`

const certificateArg = process.argv
	.map((arg) => arg.match(/--name=(?<value>.+)/i))
	.filter(Boolean)[0]
const certificateName = certificateArg
	? certificateArg.groups.value
	: process.env.npm_package_name

if (!certificateName) {
	console.error(
		"Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly."
	)
	process.exit(-1)
}

const certFilePath = path.join(baseFolder, `${certificateName}.pem`)
const keyFilePath = path.join(baseFolder, `${certificateName}.key`)

if (!fs.existsSync(".env")) {
	fs.writeFileSync(
		".env",
		`NUXT_SSL_CRT_PATH=${certFilePath}
NUXT_SSL_KEY_PATH=${keyFilePath}`
	)
} else {
	const lines = fs
		.readFileSync(".env")
		.toString()
		.split("\n")

	let hasCert,
		hasCertKey = false
	for (const line of lines) {
		if (/NUXT_SSL_CRT_PATH=.*/i.test(line)) {
			hasCert = true
		}
		if (/NUXT_SSL_KEY_PATH=.*/i.test(line)) {
			hasCertKey = true
		}
	}
	if (!hasCert) {
		fs.appendFileSync(
			".env",
			`\nNUXT_SSL_CRT_PATH=${certFilePath}`
		)
	}
	if (!hasCertKey) {
		fs.appendFileSync(
			".env",
			`\nNUXT_SSL_KEY_PATH=${keyFilePath}`
		)
	}
}
