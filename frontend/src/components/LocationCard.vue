<template>
  <div class="location-card" @click="showDetail = true">
    <div class="card-header">
      <span class="card-icon">{{ typeIcon }}</span>
      <v-chip
        :class="location.isFree ? 'chip-free' : 'chip-paid'"
        size="small"
        variant="flat"
      >
        {{ location.isFree ? 'Gratis' : 'Kostnad' }}
      </v-chip>
    </div>
    <h3 class="card-title">{{ location.name }}</h3>
    <p class="card-address">📍 {{ location.address }}</p>
    <v-chip size="small" variant="outlined" color="primary" class="card-type">
      {{ typeDisplayName }}
    </v-chip>
    <p v-if="location.description" class="card-desc">{{ truncatedDescription }}</p>
    <div class="card-footer">
      <span class="card-time">Tillagd {{ formatTimeAgo(location.createdAt) }}</span>
    </div>

    <!-- Detail dialog -->
    <v-dialog v-model="showDetail" max-width="420" transition="dialog-transition" @click:outside="showDetail = false">
      <v-card class="detail-dialog" rounded="xl" @click.stop>
        <v-card-title class="detail-header">
          <span class="detail-icon">{{ typeIcon }}</span>
          <div class="detail-title-wrap">
            <h3 class="detail-title">{{ location.name }}</h3>
            <v-chip :class="location.isFree ? 'chip-free' : 'chip-paid'" size="small" variant="flat">
              {{ location.isFree ? 'Gratis' : 'Kostnad' }}
            </v-chip>
          </div>
        </v-card-title>
        <v-card-text class="detail-body">
          <p class="detail-address">📍 {{ location.address }}</p>
          <v-chip size="small" variant="outlined" color="primary" class="mb-3">
            {{ typeDisplayName }}
          </v-chip>
          <p v-if="location.description" class="detail-desc">{{ location.description }}</p>
          <p v-else class="detail-desc text-medium-emphasis">Ingen beskrivning.</p>
          <p class="detail-time">Tillagd {{ formatTimeAgo(location.createdAt) }}</p>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn color="primary" variant="text" @click="showDetail = false">Stäng</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup>
import { computed, ref } from 'vue'

const props = defineProps({
  location: {
    type: Object,
    required: true,
  },
})

const showDetail = ref(false)

const typeIcons = {
  SecondHand: '🛍️',
  VeganCafe: '🌱',
  RecyclingCenter: '♻️',
  RepairCafe: '🔧',
  Garden: '☀️',
  BikeWorkshop: '🚲',
}

const typeNames = {
  SecondHand: 'Second-hand',
  VeganCafe: 'Vegan Café',
  RecyclingCenter: 'Återvinning',
  RepairCafe: 'Reparationscafé',
  Garden: 'Trädgård',
  BikeWorkshop: 'Cykelverkstad',
}

const typeIcon = computed(() => typeIcons[props.location.type] || '📍')
const typeDisplayName = computed(() => typeNames[props.location.type] || props.location.type)
const truncatedDescription = computed(() => {
  const d = props.location.description || ''
  return d.length > 100 ? d.slice(0, 100) + '...' : d
})

function formatTimeAgo(dateStr) {
  const date = new Date(dateStr)
  const now = new Date()
  const diff = now - date
  const days = Math.floor(diff / (1000 * 60 * 60 * 24))
  const hours = Math.floor(diff / (1000 * 60 * 60))
  const minutes = Math.floor(diff / (1000 * 60))
  if (days > 0) return `för ${days} dagar sedan`
  if (hours > 0) return `för ${hours} timmar sedan`
  if (minutes > 0) return `för ${minutes} min sedan`
  return 'nyss'
}
</script>

<style scoped>
.location-card {
  background: white;
  border-radius: 16px;
  padding: 1.25rem;
  box-shadow: 0 4px 20px rgba(27, 94, 32, 0.08);
  transition: transform 0.2s, box-shadow 0.2s;
  height: 100%;
  display: flex;
  flex-direction: column;
  cursor: pointer;
}

.location-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 30px rgba(27, 94, 32, 0.12);
}

.card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 0.75rem;
}

.card-icon {
  font-size: 1.75rem;
}

.chip-free {
  background: #E8F5E9 !important;
  color: #1B5E20 !important;
}

.chip-paid {
  background: #F5F5F5 !important;
  color: #616161 !important;
}

.card-title {
  font-size: 1.1rem;
  font-weight: 600;
  color: #1B5E20;
  margin: 0 0 0.35rem 0;
}

.card-address {
  font-size: 0.9rem;
  color: #666;
  margin: 0 0 0.5rem 0;
}

.card-type {
  margin-bottom: 0.5rem;
}

.card-desc {
  font-size: 0.9rem;
  color: #555;
  line-height: 1.5;
  flex: 1;
  margin: 0 0 0.75rem 0;
}

.card-footer {
  padding-top: 0.5rem;
  border-top: 1px solid #eee;
}

.card-time {
  font-size: 0.8rem;
  color: #999;
}

/* Detail dialog */
.detail-dialog {
  overflow: hidden;
}

.detail-header {
  display: flex;
  align-items: flex-start;
  gap: 0.75rem;
  padding: 1.25rem 1.25rem 0.5rem;
}

.detail-icon {
  font-size: 2.5rem;
}

.detail-title-wrap {
  flex: 1;
}

.detail-title {
  font-size: 1.25rem;
  font-weight: 600;
  color: #1B5E20;
  margin: 0 0 0.35rem 0;
}

.detail-body {
  padding: 0 1.25rem 1rem;
}

.detail-address {
  font-size: 0.95rem;
  color: #666;
  margin: 0 0 0.5rem 0;
}

.detail-desc {
  font-size: 0.95rem;
  line-height: 1.6;
  color: #333;
  margin: 0 0 0.5rem 0;
}

.detail-time {
  font-size: 0.85rem;
  color: #999;
  margin: 0;
}
</style>
