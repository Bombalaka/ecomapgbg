<template>
  <v-footer class="app-footer mt-auto pa-6" color="surface" border="t">
    <div class="footer-inner">
      <p class="footer-text">
        Kartdata ©
        <a href="https://www.openstreetmap.org/copyright" target="_blank" rel="noopener">OpenStreetMap</a>
        · Geocoding via
        <a href="https://nominatim.org/" target="_blank" rel="noopener">Nominatim</a>
        (<a href="https://operations.osmfoundation.org/policies/nominatim/" target="_blank" rel="noopener">policy</a>)
        ·
        <a href="https://github.com/Bombalaka/ecomapgbg" target="_blank" rel="noopener">GitHub</a>
        <span v-if="hasMapbox"> · Kartstil kan använda Mapbox</span>
      </p>
      <p class="footer-sub text-caption text-medium-emphasis mb-0">
        EcoMapGbg – återbrukskartan för Göteborg. Ingen spårning, inga annonser.
      </p>
    </div>
  </v-footer>
</template>

<script setup>
import { computed } from 'vue'

const hasMapbox = computed(() => !!import.meta.env.VITE_MAPBOX_TOKEN)
</script>

<style scoped>
.app-footer {
  flex-wrap: wrap;
  margin-top: 2rem;
}

.footer-inner {
  max-width: 1200px;
  margin: 0 auto;
  width: 100%;
  text-align: center;
}

.footer-text {
  font-size: 0.85rem;
  line-height: 1.6;
  color: #555;
  margin: 0 0 0.35rem 0;
}

.footer-text a {
  color: #1b5e20;
  text-decoration: underline;
  text-underline-offset: 2px;
}

.footer-sub {
  margin-top: 0.25rem;
}
</style>
