<template>
  <v-app class="d-flex flex-column">
    <AppNav v-if="!hideChrome" />
    <v-main
      class="app-main flex-grow-1"
      :class="{ 'app-main--full': hideChrome }"
    >
      <div v-if="!hideChrome" class="page-contained">
        <router-view v-slot="{ Component }">
          <transition name="fade-page" mode="out-in">
            <component :is="Component" />
          </transition>
        </router-view>
      </div>
      <router-view v-else v-slot="{ Component }">
        <transition name="fade-page" mode="out-in">
          <component :is="Component" />
        </transition>
      </router-view>
    </v-main>
    <AppFooter v-if="!hideChrome" />
  </v-app>
</template>

<style scoped>
.app-main {
  min-height: 0;
}

.app-main--full {
  padding: 0 !important;
}

.page-contained {
  max-width: 1200px;
  margin: 0 auto;
  padding: 1rem 1rem 1.5rem;
}

@media (min-width: 960px) {
  .page-contained {
    padding: 1.5rem 1.5rem 2rem;
  }
}

.fade-page-enter-active,
.fade-page-leave-active {
  transition: opacity 0.2s ease;
}

.fade-page-enter-from,
.fade-page-leave-to {
  opacity: 0;
}
</style>

<script setup>
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import AppNav from './components/AppNav.vue'
import AppFooter from './components/AppFooter.vue'

const route = useRoute()
const hideChrome = computed(() => route.meta.hideChrome === true)
</script>
