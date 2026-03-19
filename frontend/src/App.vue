<template>
  <v-app class="d-flex flex-column">
    <AppNav />
    <v-main class="app-main flex-grow-1">
      <v-container fluid class="pa-4 pa-md-6" style="max-width: 1200px; margin: 0 auto;">
        <router-view />
      </v-container>
    </v-main>
    <AppFooter />
  </v-app>
</template>

<style scoped>
.app-main {
  min-height: 0;
}
</style>

<script setup>
import AppNav from './components/AppNav.vue'
import AppFooter from './components/AppFooter.vue'
</script>
