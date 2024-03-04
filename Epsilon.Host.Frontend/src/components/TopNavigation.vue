<template>
	<div class="top-navigation">
		<v-row>
			<v-col cols="12" md="4">
				<a href="https://github.com/tpcly/epsilon-lti" target="_blank">
					<img
						alt="logo"
						class="top-navigation-logo"
						src="../assets/logo-white.png" />
					<div
						v-if="
							runtimeConfig.public.clientVersion.includes('Beta')
						"
						class="top-navigation-beta">
						Beta
					</div>
				</a>
			</v-col>
			<v-spacer></v-spacer>
			<slot></slot>
		</v-row>
	</div>
</template>

<script lang="ts" setup>
const runtimeConfig = useRuntimeConfig()
</script>

<style lang="scss">
.top-navigation {
	padding: 1rem 1.5rem;
	background-color: #11284c;
	width: 100%;
	border-radius: 0.5rem;

	.v-autocomplete .v-field {
		padding: 3px 6px;
	}

	.v-autocomplete {
		background-color: #ffffff;
		border-radius: 6px;
	}

	.v-autocomplete .v-input__details {
		display: none;
	}

	&-logo {
		height: 3rem;
		object-fit: contain;
	}

	a {
		position: relative;
	}

	&-beta {
		color: #ffffff;
		width: max-content;
		text-align: center;
		padding: 2px 3px;
		background-color: #848da4;
		border-radius: 6px;
		position: absolute;
		right: -20px;
		top: -40px;
	}
}
</style>
