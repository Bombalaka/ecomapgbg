<template>
  <div class="places-page">
    <header class="places-header">
      <h1 class="page-title">Eco-platser i Göteborg</h1>
      <div class="header-actions">
        <v-chip color="primary" variant="flat" size="large" class="count-chip">
          {{ locations.length }} platser
        </v-chip>
        <v-btn to="/places/add" color="primary" rounded="lg">
          Lägg till plats
        </v-btn>
      </div>
    </header>

    <v-progress-linear v-if="isLoading" indeterminate color="primary" class="mb-4" />

    <div v-else-if="locations.length > 0">
      <div class="map-section">
        <LocationMap :locations="locations" class="places-map" />
      </div>
      <h2 class="section-title">Alla platser</h2>
      <div class="places-grid">
        <LocationCard v-for="loc in locations" :key="loc.id" :location="loc" />
      </div>
    </div>

    <div v-else class="empty-state">
      <span class="empty-icon">🌱</span>
      <h3>Inga eco-platser ännu</h3>
      <p>Var först med att lägga till en plats!</p>
      <v-btn to="/places/add" color="primary" rounded="lg">Lägg till plats</v-btn>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { api } from '../services/api'
import LocationCard from '../components/LocationCard.vue'
import LocationMap from '../components/LocationMap.vue'

const locations = ref([])
const isLoading = ref(true)

onMounted(async () => {
  try {
    locations.value = await api.getLocations()
  } catch (e) {
    console.error('Error loading locations:', e)
  } finally {
    isLoading.value = false
  }
})
</script>

<style scoped>
.places-page {
  padding-bottom: 2rem;
}

.places-header {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.page-title {
  font-size: 1.5rem;
  font-weight: 600;
  color: #1B5E20;
  margin: 0;
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.count-chip {
  font-weight: 500;
}

.map-section {
  margin-bottom: 2rem;
  border-radius: 16px;
  overflow: hidden;
  box-shadow: 0 4px 20px rgba(27, 94, 32, 0.08);
}

.places-map {
  border-radius: 16px;
}

.section-title {
  font-size: 1.1rem;
  font-weight: 600;
  color: #1B5E20;
  margin-bottom: 1rem;
}

.places-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.25rem;
}

.empty-state {
  text-align: center;
  padding: 4rem 2rem;
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(27, 94, 32, 0.08);
}

.empty-icon {
  font-size: 4rem;
  display: block;
  margin-bottom: 1rem;
}

.empty-state h3 {
  font-size: 1.25rem;
  color: #333;
  margin: 0 0 0.5rem 0;
}

.empty-state p {
  color: #666;
  margin-bottom: 1.5rem;
}
</style>
