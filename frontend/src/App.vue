<template>
  <v-app>
    <AppNav />
    <v-main class="app-main">
      <v-container fluid class="pa-4 pa-md-6" style="max-width: 1200px; margin: 0 auto;">
        <router-view />
      </v-container>
    </v-main>
  </v-app>
</template>

<style scoped>
.app-main {
  min-height: 100vh;
}
</style>

<script setup>
import AppNav from './components/AppNav.vue'
</script>
