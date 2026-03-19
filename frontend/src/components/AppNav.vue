<template>
  <v-app-bar elevation="0" color="primary" density="comfortable" class="app-nav">
    <v-app-bar-title class="d-flex align-center">
      <router-link to="/" class="nav-brand">
        <span class="nav-logo">🌱</span>
        <span class="nav-title">EcoMapGBG</span>
      </router-link>
    </v-app-bar-title>
    <v-spacer />
    <v-btn variant="text" color="white" to="/" class="nav-link">Hem</v-btn>
    <v-btn variant="text" color="white" to="/places" class="nav-link">Platser</v-btn>
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
  </v-app-bar>
</template>

<script setup>
</script>

<style scoped>
.app-nav {
  border-bottom: 1px solid rgba(255,255,255,0.1);
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
