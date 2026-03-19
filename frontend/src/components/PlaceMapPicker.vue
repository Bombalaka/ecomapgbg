<template>
  <div class="picker-wrap">
    <p class="picker-hint">{{ hintText }}</p>
    <div ref="mapEl" class="picker-map"></div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch, onUnmounted } from 'vue'

const props = defineProps({
  latitude: { type: Number, default: 0 },
  longitude: { type: Number, default: 0 },
})

const emit = defineEmits(['update:latitude', 'update:longitude'])

const mapEl = ref(null)
let map = null
let marker = null

const DEFAULT_LAT = 57.7089
const DEFAULT_LNG = 11.9746

const hintText = 'Klicka på kartan för att sätta exakt plats (eller använd knappen ovan från adress).'

onMounted(async () => {
  const L = (await import('leaflet')).default
  await import('leaflet/dist/leaflet.css')

  const startLat = props.latitude && props.latitude !== 0 ? props.latitude : DEFAULT_LAT
  const startLng = props.longitude && props.longitude !== 0 ? props.longitude : DEFAULT_LNG

  map = L.map(mapEl.value).setView([startLat, startLng], 13)
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '© OpenStreetMap',
  }).addTo(map)

  if (props.latitude && props.longitude && (props.latitude !== 0 || props.longitude !== 0)) {
    marker = L.marker([props.latitude, props.longitude]).addTo(map)
  }

  map.on('click', (e) => {
    const { lat, lng } = e.latlng
    emit('update:latitude', lat)
    emit('update:longitude', lng)
    if (marker) {
      marker.setLatLng([lat, lng])
    } else {
      marker = L.marker([lat, lng]).addTo(map)
    }
  })
})

watch(
  () => [props.latitude, props.longitude],
  async () => {
    if (!map || !props.latitude || !props.longitude) return
    if (props.latitude === 0 && props.longitude === 0) return
    const L = (await import('leaflet')).default
    const lat = props.latitude
    const lng = props.longitude
    if (marker) {
      marker.setLatLng([lat, lng])
    } else {
      marker = L.marker([lat, lng]).addTo(map)
    }
    map.setView([lat, lng], 15)
  }
)

onUnmounted(() => {
  if (map) {
    map.remove()
    map = null
    marker = null
  }
})
</script>

<style scoped>
.picker-wrap {
  margin-top: 0.5rem;
}

.picker-hint {
  font-size: 0.85rem;
  color: #666;
  margin: 0 0 0.5rem 0;
}

.picker-map {
  height: 260px;
  width: 100%;
  border-radius: 12px;
  overflow: hidden;
  border: 1px solid #e0e0e0;
}

:deep(.leaflet-container) {
  font-family: inherit;
}
</style>
