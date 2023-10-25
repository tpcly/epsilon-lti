<template>
    <NuxtLayout :name="layout">
        <NuxtPage />
    </NuxtLayout>
</template>

<script setup lang="ts">
const layout = "default";

// Process LTI launch
if (process.server) {
    const runtimeConfig = useRuntimeConfig()
    const state = useState("id_token")
    
    if (runtimeConfig.overrideIdentityToken !== "") {
        state.value = runtimeConfig.overrideIdentityToken
    } else {
        const { readCallback } = useLti()
        const event = useRequestEvent()
        const callback = await readCallback(event)

        state.value = callback.idToken
    }
}
</script>

<style lang="scss">
@import "assets/styles/resets";
@import "assets/styles/main";

html,
body {
    width: 100%;
    max-width: 1366px;
    height: 100vh;
    margin: 0 auto;
}

body {
    background: $color-body;
    font-family: Inter, system-ui, Avenir, Helvetica, Arial, sans-serif;
    line-height: 1.5;
    font-weight: 400;
    color: $color-text;
}

b,
strong {
    font-weight: bold;
}

* {
    box-sizing: border-box;
}
</style>
