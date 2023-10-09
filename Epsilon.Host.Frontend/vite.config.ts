import { defineConfig, loadEnv, UserConfigExport } from "vite"
import vue from "@vitejs/plugin-vue"
import { resolve } from "path"
import { readFileSync } from "fs"

export default ({ mode }: { mode: string }): UserConfigExport => {
    process.env = { ...process.env, ...loadEnv(mode, process.cwd()) }
    const certificate = process.env.VITE_SSL_CRT_FILE
    const key = process.env.VITE_SSL_KEY_FILE
    const inDevelopment = process.env.VITE_IN_DEVELOPMENT
    let serverConfig = {}
    if(inDevelopment == 'true')
    {
        serverConfig = {
            https: {
                cert: certificate ? readFileSync(certificate) : undefined,
                key: key ? readFileSync(key) : undefined,
            },
            port: 8000,
        }
        console.log("https")
    }else{
        serverConfig =  {
            http: {},
            port: 8000,
        }
        console.log("http")
        console.log(inDevelopment)
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
