<template>
  <div class="location-map-wrapper">
    <div ref="mapContainer" class="map-container"></div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'

const props = defineProps({
  locations: {
    type: Array,
    default: () => [],
  },
})

const mapboxToken = import.meta.env.VITE_MAPBOX_TOKEN
const mapContainer = ref(null)
let map = null
let markers = []
let mapType = null // 'mapbox' | 'leaflet'

onMounted(() => {
  if (!mapContainer.value) return
  if (mapboxToken) {
    initMapbox()
  } else {
    initLeaflet()
  }
})

async function initMapbox() {
  const mapboxgl = (await import('mapbox-gl')).default
  await import('mapbox-gl/dist/mapbox-gl.css')
  mapboxgl.accessToken = mapboxToken
  map = new mapboxgl.Map({
    container: mapContainer.value,
    style: 'mapbox://styles/mapbox/light-v11',
    center: [11.9746, 57.7089],
    zoom: 13,
  })
  map.addControl(new mapboxgl.NavigationControl(), 'top-right')
  mapType = 'mapbox'
  updateMarkers()
}

async function initLeaflet() {
  const L = (await import('leaflet')).default
  await import('leaflet/dist/leaflet.css')
  map = L.map(mapContainer.value).setView([57.7089, 11.9746], 13)
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '© OpenStreetMap contributors',
  }).addTo(map)
  mapType = 'leaflet'
  updateMarkers()
}

async function updateMarkers() {
  if (!map || !mapType || !props.locations?.length) return

  if (mapType === 'mapbox') {
    const mapboxgl = (await import('mapbox-gl')).default
    markers.forEach((m) => m.remove())
    markers = []
    const bounds = new mapboxgl.LngLatBounds()
    for (const loc of props.locations) {
      if (loc.latitude == null || loc.longitude == null) continue
      const el = document.createElement('div')
      el.innerHTML = getIcon(loc.type)
      el.style.fontSize = '24px'
      el.style.cursor = 'pointer'
      const popup = new mapboxgl.Popup({ offset: 25 }).setHTML(popupHtml(loc))
      const marker = new mapboxgl.Marker(el)
        .setLngLat([loc.longitude, loc.latitude])
        .setPopup(popup)
        .addTo(map)
      markers.push(marker)
      bounds.extend([loc.longitude, loc.latitude])
    }
    if (markers.length > 0) {
      map.fitBounds(bounds, { padding: 50, maxZoom: 14 })
    }
  } else {
    const L = (await import('leaflet')).default
    markers.forEach((m) => m.remove())
    markers = []
    const bounds = []
    for (const loc of props.locations) {
      if (loc.latitude == null || loc.longitude == null) continue
      const icon = L.divIcon({
        html: `<div style="font-size: 24px;">${getIcon(loc.type)}</div>`,
        className: 'custom-marker',
        iconSize: [30, 30],
        iconAnchor: [15, 15],
      })
      const marker = L.marker([loc.latitude, loc.longitude], { icon })
        .bindPopup(popupHtml(loc), { maxWidth: 260 })
        .addTo(map)
      markers.push(marker)
      bounds.push([loc.latitude, loc.longitude])
    }
    if (markers.length > 0) {
      map.fitBounds(bounds, { padding: [50, 50], maxZoom: 14 })
    }
  }
}

watch(
  () => props.locations,
  () => updateMarkers(),
  { deep: true }
)

function getIcon(type) {
  const icons = {
    SecondHand: '🛍',
    VeganCafe: '🌱',
    RecyclingCenter: '♻️',
    RepairCafe: '🔧',
    Garden: '☀️',
    BikeWorkshop: '🚲',
  }
  return icons[type] || '📍'
}

function popupHtml(loc) {
  const typeNames = {
    SecondHand: 'Second-hand',
    VeganCafe: 'Vegan Café',
    RecyclingCenter: 'Återvinning',
    RepairCafe: 'Reparationscafé',
    Garden: 'Trädgård',
    BikeWorkshop: 'Cykelverkstad',
  }
  const typeName = typeNames[loc.type] || loc.type
  const desc = loc.description
    ? `<p style="margin: 8px 0 0 0; font-size: 13px; color: #555;">${escapeHtml(loc.description)}</p>`
    : ''
  return `
    <div style="min-width: 200px; padding: 4px;">
      <h6 style="margin: 0 0 6px 0; font-size: 15px;">${escapeHtml(loc.name)}</h6>
      <p style="margin: 0; font-size: 12px;"><strong>📍 ${escapeHtml(loc.address)}</strong></p>
      <p style="margin: 4px 0 0 0; font-size: 12px;">${loc.isFree ? '🆓 Gratis' : '💰 Kostnad'} · ${typeName}</p>
      ${desc}
    </div>
  `
}

function escapeHtml(s) {
  if (!s) return ''
  const div = document.createElement('div')
  div.textContent = s
  return div.innerHTML
}
</script>

<style scoped>
.location-map-wrapper {
  position: relative;
  min-height: 400px;
  border-radius: 16px;
  overflow: hidden;
  background: #e8f5e9;
}

.map-container {
  width: 100%;
  height: 400px;
}

:deep(.leaflet-popup-content-wrapper) {
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
}

:deep(.leaflet-popup-content) {
  margin: 12px 14px;
}

:deep(.leaflet-container) {
  font-family: var(--font-body);
}

:deep(.custom-marker) {
  background: none !important;
  border: none !important;
}
</style>
