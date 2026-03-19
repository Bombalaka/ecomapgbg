<template>
  <div class="places-page">
    <header class="places-header">
      <h1 class="page-title">Eco-platser i Göteborg</h1>
      <div class="header-actions">
        <v-chip color="primary" variant="flat" size="large" class="count-chip">
          {{ filteredLocations.length }}{{ hasActiveFilter ? ` / ${locations.length}` : '' }} platser
        </v-chip>
        <v-btn to="/places/add" color="primary" rounded="lg">
          Lägg till plats
        </v-btn>
      </div>
    </header>

    <!-- Search + filters -->
    <div v-if="!isLoading && locations.length > 0" class="filters mb-4">
      <v-text-field
        v-model="searchQuery"
        label="Sök namn eller adress"
        variant="outlined"
        density="comfortable"
        hide-details
        clearable
        prepend-inner-icon="mdi-magnify"
        class="search-field"
      />
      <div class="filter-chips">
        <v-chip
          :variant="filterType === null ? 'flat' : 'outlined'"
          :color="filterType === null ? 'primary' : undefined"
          class="mr-2 mb-2"
          @click="filterType = null"
        >
          Alla
        </v-chip>
        <v-chip
          v-for="opt in typeFilters"
          :key="opt.value"
          :variant="filterType === opt.value ? 'flat' : 'outlined'"
          :color="filterType === opt.value ? 'primary' : undefined"
          class="mr-2 mb-2"
          @click="filterType = filterType === opt.value ? null : opt.value"
        >
          {{ opt.label }}
        </v-chip>
      </div>
    </div>

    <v-progress-linear v-if="isLoading" indeterminate color="primary" class="mb-4" />

    <div v-else-if="locations.length > 0">
      <div v-if="filteredLocations.length > 0">
        <div class="map-section">
          <LocationMap :locations="filteredLocations" class="places-map" />
        </div>
        <h2 class="section-title">Alla platser</h2>
        <div class="places-grid">
          <LocationCard v-for="loc in filteredLocations" :key="loc.id" :location="loc" />
        </div>
      </div>

      <!-- Filter/search produced no results -->
      <div v-else class="empty-state empty-filtered">
        <span class="empty-icon">🔍</span>
        <h3>Inga platser matchar</h3>
        <p>Prova annat sökord eller rensa filtren.</p>
        <v-btn color="primary" variant="tonal" rounded="lg" class="mr-2" @click="resetFilters">
          Rensa sök &amp; filter
        </v-btn>
        <v-btn to="/places/add" color="primary" rounded="lg">Lägg till plats</v-btn>
      </div>
    </div>

    <!-- No data in database -->
    <div v-else class="empty-state">
      <span class="empty-icon">🌱</span>
      <h3>Kartan är tom – bli först!</h3>
      <p class="empty-lead">
        Det finns inga platser i databasen än. Lägg till en second-hand-butik, ett reparationscafé eller annat
        som hjälper Göteborg att återbruka.
      </p>
      <div class="empty-actions">
        <v-btn to="/places/add" color="primary" size="large" rounded="lg">Lägg till första platsen</v-btn>
        <v-btn to="/help" variant="text" rounded="lg">Så funkar det</v-btn>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { api } from '../services/api'
import LocationCard from '../components/LocationCard.vue'
import LocationMap from '../components/LocationMap.vue'

const locations = ref([])
const isLoading = ref(true)
const searchQuery = ref('')
const filterType = ref(null)

const typeFilters = [
  { value: 'SecondHand', label: 'Second-hand' },
  { value: 'VeganCafe', label: 'Vegan' },
  { value: 'RecyclingCenter', label: 'Återvinning' },
  { value: 'RepairCafe', label: 'Reparation' },
  { value: 'Garden', label: 'Trädgård' },
  { value: 'BikeWorkshop', label: 'Cykel' },
]

const filteredLocations = computed(() => {
  let list = locations.value
  if (filterType.value) {
    list = list.filter((l) => l.type === filterType.value)
  }
  const q = searchQuery.value?.trim().toLowerCase()
  if (q) {
    list = list.filter((l) => {
      const name = (l.name || '').toLowerCase()
      const addr = (l.address || '').toLowerCase()
      const desc = (l.description || '').toLowerCase()
      return name.includes(q) || addr.includes(q) || desc.includes(q)
    })
  }
  return list
})

const hasActiveFilter = computed(
  () => !!filterType.value || !!searchQuery.value?.trim()
)

function resetFilters() {
  searchQuery.value = ''
  filterType.value = null
}

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
  margin-bottom: 1rem;
}

.page-title {
  font-size: 1.5rem;
  font-weight: 600;
  color: #1b5e20;
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

.filters {
  background: #fff;
  padding: 1rem 1.25rem;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(27, 94, 32, 0.08);
}

.search-field {
  max-width: 400px;
  margin-bottom: 0.5rem;
}

.filter-chips {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
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
  color: #1b5e20;
  margin-bottom: 1rem;
}

.places-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.25rem;
}

.empty-state {
  text-align: center;
  padding: 3rem 1.5rem;
  background: #fff;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(27, 94, 32, 0.08);
}

.empty-filtered {
  padding: 2.5rem 1.5rem;
}

.empty-icon {
  font-size: 3.5rem;
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
  margin-bottom: 1.25rem;
  max-width: 460px;
  margin-left: auto;
  margin-right: auto;
}

.empty-lead {
  line-height: 1.55;
}

.empty-actions {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  justify-content: center;
  align-items: center;
}
</style>
