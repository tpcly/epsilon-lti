<template>
	<button class="refresh-button" @click="resetDates">↻</button>
	<div ref="dateRangeMenu" class="date-range-menu">
		<button @click="toggleMenu">Date range</button>
		<div class="menu-content" :class="{ 'menu-visible': menuVisible }">
			<div class="date-range-filter">
				<div class="date-input">
					<label for="startDate">Start date:</label>
					<input
						id="startDate"
						v-model="startDate"
						type="date"
						@input="filterItems" />
				</div>
				<div class="date-input">
					<label for="endDate">End date:</label>
					<input
						id="endDate"
						v-model="endDate"
						type="date"
						@input="filterItems" />
				</div>
			</div>
		</div>
	</div>
</template>

<script>
export default {
	props: {
		items: Array,
	},
	data() {
		return {
			startDate: null,
			endDate: null,
			menuVisible: false,
			originalItems: [],
			hasSetInitialItems: false,
		}
	},
	beforeUpdate() {
		if (this.items && this.items.length > 0 && !this.hasSetInitialItems) {
			this.setInitialItems()
			this.hasSetInitialItems = true
		}
	},
	methods: {
		setInitialItems() {
			this.originalItems = this.items
		},
		filterItems() {
			if (this.startDate && this.endDate) {
				const selectedStartDate = new Date(this.startDate)
				const selectedEndDate = new Date(this.endDate)

				const filteredTerms = this.items.filter((term) => {
					const termStartDate = new Date(term.start_at)
					const termEndDate = new Date(term.end_at)
					return (
						termStartDate >= selectedStartDate &&
						termEndDate <= selectedEndDate
					)
				})
				this.$store.commit("setUserTerms", filteredTerms)
			}
		},
		resetDates() {
			this.$store.commit("setUserTerms", this.originalItems)
		},
		toggleMenu() {
			this.menuVisible = !this.menuVisible
			if (this.menuVisible) {
				document.addEventListener("click", this.closeMenuOnClickOutside)
			} else {
				document.removeEventListener(
					"click",
					this.closeMenuOnClickOutside
				)
			}
		},
		closeMenuOnClickOutside(event) {
			if (
				this.$refs.dateRangeMenu &&
				!this.$refs.dateRangeMenu.contains(event.target)
			) {
				this.menuVisible = false
				document.removeEventListener(
					"click",
					this.closeMenuOnClickOutside
				)
			}
		},
	},
}
</script>

<style scoped>
.date-range-menu {
	position: relative;
	display: inline-block;
}

.menu-content {
	position: absolute;
	top: 100%;
	left: 0;
	display: none;
	background-color: #fff;
	padding: 0.75rem;
	box-shadow: 0px 1px 2px rgba(0, 0, 0, 0.2);
	border-radius: 6px;
	border: 1px solid #d8d8d8;
}

.menu-visible {
	display: block;
}

.date-range-menu button {
	border: none;
	cursor: pointer;
	height: 45px;
	background-color: #fff;
	border-radius: 7px;
	font-family: inherit;
	font-size: 1rem;
	font-weight: 400;
	width: 135%;
	vertical-align: middle;
}

.date-input {
	margin-bottom: 10px;
}

.date-input label {
	display: block;
	margin-bottom: 5px;
}

.date-input input[type="date"] {
	font-family: inherit;
	font-size: 1rem;
	font-weight: 400;
}

.refresh-button {
	border: none;
	font-family: inherit;
	font-size: 1.3rem;
	font-weight: 400;
	border-radius: 7px;
	height: 45px;
	background-color: #fff;
	margin-right: 5px;
	vertical-align: middle;
	cursor: pointer;
}
</style>
