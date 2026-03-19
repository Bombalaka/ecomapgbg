<template>
  <div class="home">
    <!-- Calm hero (motion lives on / landing page) -->
    <section class="hero">
      <div class="hero-content">
        <span class="hero-badge">Återbrukskartan</span>
        <h1 class="hero-title">EcoMapGbg</h1>
        <p class="hero-subtitle">
          Discover Gothenburg's friendliest eco-places: second-hand shops, vegan cafés,
          recycling stations, repair spots, and community gardens.
        </p>
        <p class="hero-tagline">Made with warmth for everyone — from kids to grandmas</p>
        <v-btn to="/places" size="x-large" class="hero-cta" color="primary" rounded="xl" elevation="0">
          Explore the map
        </v-btn>
        <p class="hero-back">
          <router-link
            :to="{ name: 'Landing', query: { replay: '1' } }"
            class="hero-back-link"
          >
            Till startsidan (video)
          </router-link>
        </p>
      </div>
    </section>

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

    <section class="categories">
      <h2 class="section-title">What you'll find</h2>
      <div class="category-grid">
        <div v-for="(cat, idx) in categories" :key="cat.name + idx" class="category-card">
          <span class="category-icon">{{ cat.icon }}</span>
          <span class="category-name">{{ cat.name }}</span>
        </div>
      </div>
    </section>

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

    <section class="cta">
      <v-btn to="/places" color="primary" size="large" rounded="xl">View all places</v-btn>
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
  background: radial-gradient(ellipse 120% 80% at 50% -20%, rgba(165, 214, 167, 0.28), transparent 55%),
    linear-gradient(180deg, #f3faf4 0%, #fafcfa 45%, #fff 100%);
  margin: -0.5rem -4px 0;
  padding: 0 4px 3.5rem;
}

.hero {
  position: relative;
  overflow: hidden;
  background: linear-gradient(135deg, #1b5e20 0%, #2e7d32 45%, #2e7d32 100%);
  border-radius: 22px;
  padding: 2.75rem 1.5rem;
  margin-bottom: 2.5rem;
  color: white;
  text-align: center;
}

.hero-content {
  position: relative;
  z-index: 1;
  max-width: 540px;
  margin: 0 auto;
}

.hero-badge {
  display: inline-block;
  background: rgba(255, 255, 255, 0.2);
  padding: 0.3rem 0.9rem;
  border-radius: 999px;
  font-size: 0.82rem;
  font-weight: 500;
  margin-bottom: 0.85rem;
  letter-spacing: 0.4px;
}

.hero-title {
  font-size: 2.4rem;
  font-weight: 700;
  margin: 0 0 0.65rem;
  letter-spacing: -0.5px;
}

.hero-subtitle {
  font-size: 1.05rem;
  opacity: 0.95;
  margin: 0 0 0.45rem;
  line-height: 1.6;
}

.hero-tagline {
  font-size: 0.92rem;
  margin: 0 0 1.35rem;
  opacity: 0.88;
}

.hero-cta {
  text-transform: none;
  font-weight: 600;
  padding: 0.45rem 1.75rem !important;
}

.hero-back {
  margin: 1rem 0 0;
  font-size: 0.85rem;
  opacity: 0.85;
}

.hero-back-link {
  color: #e8f5e9;
  text-decoration: underline;
  text-underline-offset: 3px;
}

.hero-back-link:hover {
  color: #fff;
}

.section-title {
  font-size: 1.2rem;
  font-weight: 600;
  color: #1b5e20;
  margin-bottom: 1.1rem;
}

.categories {
  margin-bottom: 2.25rem;
}

.category-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(140px, 1fr));
  gap: 1rem;
}

.category-card {
  background: white;
  border-radius: 16px;
  padding: 1.2rem;
  text-align: center;
  box-shadow: 0 4px 18px rgba(27, 94, 32, 0.07);
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.category-card:hover {
  transform: translateY(-3px);
  box-shadow: 0 8px 26px rgba(27, 94, 32, 0.12);
}

.category-icon {
  font-size: 1.85rem;
  display: block;
  margin-bottom: 0.4rem;
}

.category-name {
  font-size: 0.88rem;
  font-weight: 500;
  color: #333;
}

.stats {
  margin-bottom: 2.25rem;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1rem;
}

.stat-card {
  background: white;
  border-radius: 16px;
  padding: 1.35rem;
  text-align: center;
  box-shadow: 0 4px 18px rgba(27, 94, 32, 0.07);
}

.stat-value {
  display: block;
  font-size: 1.85rem;
  font-weight: 700;
  color: #1b5e20;
  margin-bottom: 0.2rem;
}

.stat-label {
  font-size: 0.9rem;
  color: #666;
}

.cta {
  text-align: center;
}

@media (max-width: 600px) {
  .hero {
    padding: 2rem 1.2rem;
  }
  .hero-title {
    font-size: 2rem;
  }
  .stats-grid {
    grid-template-columns: 1fr;
  }
}
</style>
