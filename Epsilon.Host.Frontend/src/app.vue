<template>
	<NuxtLayout>
		<v-app>
			<NuxtPage />
		</v-app>
	</NuxtLayout>
</template>

<script lang="ts" setup></script>
<style lang="scss">
$body-font-family: Inter, system-ui, Avenir, Helvetica, Arial, sans-serif;
@use "assets/styles/resets";
@use "assets/styles/main";
@use "assets/styles/variables/color" as vars;

html,
body {
	width: 100%;
	max-width: 1366px;
	height: 100vh;
	margin: 0 auto;
	font-family: Inter, system-ui, Avenir, Helvetica, Arial, sans-serif !important;
}

body {
	background: vars.$color-body;
	line-height: 1.5;
	font-weight: 400;
	color: vars.$color-text;
}

b,
strong {
	font-weight: bold;
}

* {
	box-sizing: border-box;
}
</style>
