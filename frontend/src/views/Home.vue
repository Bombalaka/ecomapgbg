<template>
  <div class="home">
    <!-- Hero -->
    <section class="hero">
      <div class="hero-content">
        <span class="hero-badge">Återbrukskartan</span>
        <h1 class="hero-title">EcoMapGbg</h1>
        <p class="hero-subtitle">
          Discover Gothenburg's friendliest eco-places: second-hand shops, vegan cafés,
          recycling stations, repair spots, and community gardens.
        </p>
        <p class="hero-tagline">Made with warmth for everyone — from kids to grandmas</p>
        <v-btn
          to="/places"
          size="x-large"
          class="hero-cta"
          color="primary"
          rounded="xl"
          elevation="0"
        >
          Explore the map
        </v-btn>
      </div>
    </section>

    <!-- Empty DB CTA -->
    <v-alert
      v-if="loaded && locationCount === 0"
      type="info"
      variant="tonal"
      rounded="xl"
      class="mb-6"
      prominent
    >
      <strong>Ingen plats i databasen än.</strong>
      Börja med att lägga till en återbruksplats – då syns den på kartan för alla.
      <template #append>
        <v-btn to="/places/add" color="primary" rounded="lg">Lägg till plats</v-btn>
      </template>
    </v-alert>

    <!-- Categories -->
    <section class="categories">
      <h2 class="section-title">What you'll find</h2>
      <div class="category-grid">
        <div
          v-for="cat in categories"
          :key="cat.name"
          class="category-card"
        >
          <span class="category-icon">{{ cat.icon }}</span>
          <span class="category-name">{{ cat.name }}</span>
        </div>
      </div>
    </section>

    <!-- Stats -->
    <section v-if="locationCount > 0" class="stats">
      <h2 class="section-title">Our impact so far</h2>
      <div class="stats-grid">
        <div class="stat-card">
          <span class="stat-value">{{ locationCount }}</span>
          <span class="stat-label">Eco places</span>
        </div>
        <div class="stat-card">
          <span class="stat-value">♻️</span>
          <span class="stat-label">Sustainable living</span>
        </div>
        <div class="stat-card">
          <span class="stat-value">🌍</span>
          <span class="stat-label">Better future</span>
        </div>
      </div>
    </section>

    <!-- CTA -->
    <section class="cta">
      <v-btn to="/places" color="primary" size="large" rounded="xl">
        View all places
      </v-btn>
    </section>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { api } from '../services/api'

const locationCount = ref(0)
const loaded = ref(false)
const categories = [
  { icon: '♻️', name: 'Second-hand' },
  { icon: '🌱', name: 'Vegan cafés' },
  { icon: '♻️', name: 'Recycling' },
  { icon: '🚲', name: 'Repairs' },
  { icon: '☀️', name: 'Gardens' },
]

onMounted(async () => {
  try {
    const locations = await api.getLocations()
    locationCount.value = locations?.length ?? 0
  } catch (e) {
    console.error('Error loading location count:', e)
  } finally {
    loaded.value = true
  }
})
</script>

<style scoped>
.home {
  padding-bottom: 4rem;
}

.hero {
  background: linear-gradient(135deg, #1B5E20 0%, #2E7D32 50%, #388E3C 100%);
  border-radius: 24px;
  padding: 3rem 2rem;
  margin-bottom: 3rem;
  color: white;
  text-align: center;
}

.hero-badge {
  display: inline-block;
  background: rgba(255,255,255,0.2);
  padding: 0.35rem 1rem;
  border-radius: 999px;
  font-size: 0.85rem;
  font-weight: 500;
  margin-bottom: 1rem;
  letter-spacing: 0.5px;
}

.hero-title {
  font-size: 2.5rem;
  font-weight: 700;
  margin: 0 0 0.75rem 0;
  letter-spacing: -0.5px;
}

.hero-subtitle {
  font-size: 1.1rem;
  opacity: 0.95;
  max-width: 520px;
  margin: 0 auto 0.5rem;
  line-height: 1.6;
}

.hero-tagline {
  font-size: 0.95rem;
  opacity: 0.85;
  margin-bottom: 1.5rem;
}

.hero-cta {
  text-transform: none;
  font-weight: 600;
  padding: 0.5rem 2rem !important;
}

.section-title {
  font-size: 1.25rem;
  font-weight: 600;
  color: #1B5E20;
  margin-bottom: 1.25rem;
}

.categories {
  margin-bottom: 2.5rem;
}

.category-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(140px, 1fr));
  gap: 1rem;
}

.category-card {
  background: white;
  border-radius: 16px;
  padding: 1.25rem;
  text-align: center;
  box-shadow: 0 4px 20px rgba(27, 94, 32, 0.08);
  transition: transform 0.2s, box-shadow 0.2s;
}

.category-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 30px rgba(27, 94, 32, 0.12);
}

.category-icon {
  font-size: 2rem;
  display: block;
  margin-bottom: 0.5rem;
}

.category-name {
  font-size: 0.9rem;
  font-weight: 500;
  color: #333;
}

.stats {
  margin-bottom: 2.5rem;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1rem;
}

.stat-card {
  background: white;
  border-radius: 16px;
  padding: 1.5rem;
  text-align: center;
  box-shadow: 0 4px 20px rgba(27, 94, 32, 0.08);
}

.stat-value {
  display: block;
  font-size: 2rem;
  font-weight: 700;
  color: #1B5E20;
  margin-bottom: 0.25rem;
}

.stat-label {
  font-size: 0.95rem;
  color: #666;
}

.cta {
  text-align: center;
}

@media (max-width: 600px) {
  .hero {
    padding: 2rem 1.25rem;
  }
  .hero-title {
    font-size: 2rem;
  }
  .stats-grid {
    grid-template-columns: 1fr;
  }
}
</style>
