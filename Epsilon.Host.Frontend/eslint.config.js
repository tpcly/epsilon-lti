import globals from "globals"
import tseslint from "typescript-eslint"
import pluginVue from "eslint-plugin-vue"
import eslintConfigPrettier from "eslint-config-prettier"

export default [
	{ files: ["**/*.{mjs,cjs,ts,vue}"] },
	{ languageOptions: { globals: globals.browser } },
	...tseslint.configs.recommended,
	...pluginVue.configs["flat/recommended"],
	...pluginVue.configs["flat/strongly-recommended"],
	...pluginVue.configs["flat/essential"],

	{
		files: ["*.vue", "**/*.vue"],
		languageOptions: {
			parserOptions: {
				parser: "@typescript-eslint/parser",
			},
		},
	},
	eslintConfigPrettier,
	{
		rules: {
			/* Possible consideration to turn on in the future */
			"@typescript-eslint/no-non-null-asserted-optional-chain": "off",
			"vue/multi-word-component-names": "off",
			/**/
			"vue/no-template-shadow": "error",
			"@typescript-eslint/no-unused-vars": "error",
			"@typescript-eslint/explicit-function-return-type": "error",
			"@typescript-eslint/explicit-module-boundary-types": "error",
			indent: ["error", "tab"],
			"vue/component-tags-order": [
				"error",
				{
					order: ["template", "script", "style"],
				},
			],
			"vue/html-indent": ["error", "tab"],
		},
	},
	{
		ignores: [
			"node_modules",
			"dist",
			"public",
			".nuxt",
			"aspnetcore-*",
			".gitignore",
			"api.generated.ts",
			"pnpm-lock.yaml",
		],
	},
]
