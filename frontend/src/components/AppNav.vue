<template>
  <div class="nav-wrap">
    <v-navigation-drawer
      v-model="drawer"
      temporary
      location="start"
      width="280"
      class="mobile-drawer"
    >
      <v-list density="comfortable" nav class="pa-2">
        <v-list-item
          v-for="item in menuItems"
          :key="item.to"
          :to="item.to"
          :title="item.title"
          :prepend-icon="item.icon"
          rounded="lg"
          color="primary"
          @click="drawer = false"
        />
      </v-list>
      <div class="drawer-cta pa-3">
        <v-btn to="/places/add" color="primary" block rounded="lg" @click="drawer = false">
          Lägg till plats
        </v-btn>
      </div>
    </v-navigation-drawer>

    <v-app-bar elevation="0" color="primary" density="comfortable" class="app-nav">
      <v-app-bar-nav-icon
        v-if="mobileNav"
        class="text-white"
        aria-label="Öppna meny"
        @click="drawer = true"
      />
      <v-app-bar-title class="d-flex align-center flex-shrink-1">
        <router-link to="/" class="nav-brand" @click="drawer = false">
          <span class="nav-logo">🌱</span>
          <span class="nav-title d-none d-sm-inline">EcoMapGBG</span>
        </router-link>
      </v-app-bar-title>
      <v-spacer />
      <!-- Desktop / tablet: inline links -->
      <template v-if="!mobileNav">
        <v-btn variant="text" color="white" to="/" class="nav-link">Hem</v-btn>
        <v-btn variant="text" color="white" to="/places" class="nav-link">Platser</v-btn>
        <v-btn variant="text" color="white" to="/events" class="nav-link">Händelser</v-btn>
        <v-btn variant="text" color="white" to="/about" class="nav-link">Om</v-btn>
        <v-btn variant="text" color="white" to="/help" class="nav-link">Hjälp</v-btn>
        <v-btn
          to="/places/add"
          color="white"
          variant="outlined"
          class="nav-add-btn"
          rounded="lg"
        >
          Lägg till plats
        </v-btn>
      </template>
    </v-app-bar>
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue'
import { useDisplay } from 'vuetify'

const drawer = ref(false)
const { mdAndDown } = useDisplay()

/** Hamburger + drawer below md breakpoint (~960px) */
const mobileNav = computed(() => mdAndDown.value)

const menuItems = [
  { title: 'Hem', to: '/', icon: 'mdi-home-outline' },
  { title: 'Platser', to: '/places', icon: 'mdi-map-marker-outline' },
  { title: 'Händelser', to: '/events', icon: 'mdi-calendar-blank-outline' },
  { title: 'Om projektet', to: '/about', icon: 'mdi-information-outline' },
  { title: 'Hjälp', to: '/help', icon: 'mdi-help-circle-outline' },
]

watch(mobileNav, (isMobile) => {
  if (!isMobile) drawer.value = false
})
</script>

<style scoped>
.nav-wrap {
  position: relative;
}

.app-nav {
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.mobile-drawer :deep(.v-list-item--active) {
  font-weight: 600;
}

.drawer-cta {
  border-top: 1px solid rgba(0, 0, 0, 0.08);
}

.nav-brand {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  text-decoration: none;
  color: white;
  font-weight: 600;
  font-size: 1.1rem;
}

.nav-logo {
  font-size: 1.5rem;
}

.nav-title {
  letter-spacing: -0.3px;
}

.nav-link {
  text-transform: none;
  font-weight: 500;
}

.nav-add-btn {
  text-transform: none;
  font-weight: 600;
}
</style>