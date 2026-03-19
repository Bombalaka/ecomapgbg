<template>
  <div class="add-place-page">
    <h1 class="page-title">Lägg till eco-plats</h1>
    <p class="page-subtitle">Adress räcker oftast — vi söker koordinater åt dig. Du kan finjustera på kartan.</p>

    <v-card class="form-card" elevation="0">
      <v-card-text>
        <v-form ref="formRef" v-model="valid" @submit.prevent="handleSubmit">
          <v-text-field
            v-model="form.name"
            label="Namn"
            placeholder="t.ex. Gröna Caféet"
            :rules="[v => !!v || 'Namn krävs']"
            variant="outlined"
            density="comfortable"
            class="mb-3"
            hide-details
          />
          <v-text-field
            v-model="form.address"
            label="Adress"
            placeholder="t.ex. Vasagatan 10, Göteborg"
            :rules="[v => !!v || 'Adress krävs']"
            variant="outlined"
            density="comfortable"
            class="mb-3"
            hide-details
          />
          <div class="d-flex flex-wrap gap-2 mb-4">
            <v-btn
              color="secondary"
              variant="tonal"
              rounded="lg"
              :loading="geocoding"
              :disabled="!form.address?.trim()"
              @click="geocodFromAddress"
            >
              Hitta från adress
            </v-btn>
            <span v-if="coordsHint" class="align-self-center text-caption text-medium-emphasis">{{ coordsHint }}</span>
          </div>

          <v-select
            v-model="form.type"
            :items="typeOptions"
            label="Kategori"
            :rules="[v => !!v || 'Välj kategori']"
            variant="outlined"
            density="comfortable"
            class="mb-3"
            hide-details
          />
          <v-textarea
            v-model="form.description"
            label="Beskrivning"
            rows="3"
            variant="outlined"
            density="comfortable"
            class="mb-3"
            hide-details
          />

          <h3 class="section-label">Plats på kartan</h3>
          <PlaceMapPicker
            :latitude="form.latitude"
            :longitude="form.longitude"
            @update:latitude="v => (form.latitude = v)"
            @update:longitude="v => (form.longitude = v)"
          />

          <v-row class="mt-2">
            <v-col cols="12" md="6">
              <v-text-field
                v-model.number="form.latitude"
                label="Latitud (valfritt manuellt)"
                type="number"
                step="0.000001"
                variant="outlined"
                density="comfortable"
                hide-details
              />
            </v-col>
            <v-col cols="12" md="6">
              <v-text-field
                v-model.number="form.longitude"
                label="Longitud (valfritt manuellt)"
                type="number"
                step="0.000001"
                variant="outlined"
                density="comfortable"
                hide-details
              />
            </v-col>
          </v-row>

          <v-checkbox v-model="form.isFree" label="Gratis att använda" color="primary" class="mt-2" hide-details />
          <v-alert v-if="message" :type="messageType" class="mt-4" variant="tonal">{{ message }}</v-alert>
          <v-btn type="submit" color="primary" size="large" rounded="lg" class="mt-4" :loading="submitting">
            Skicka
          </v-btn>
        </v-form>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { api } from '../services/api'
import PlaceMapPicker from '../components/PlaceMapPicker.vue'

const router = useRouter()
const formRef = ref(null)
const valid = ref(false)
const submitting = ref(false)
const geocoding = ref(false)
const message = ref('')
const messageType = ref('success')
const coordsHint = ref('')

const form = reactive({
  name: '',
  address: '',
  type: '',
  description: '',
  latitude: 0,
  longitude: 0,
  isFree: true,
  tags: [],
})

const typeOptions = [
  { title: 'Second-hand', value: 'SecondHand' },
  { title: 'Vegan Café', value: 'VeganCafe' },
  { title: 'Återvinningscentral', value: 'RecyclingCenter' },
  { title: 'Reparationscafé', value: 'RepairCafe' },
  { title: 'Trädgård', value: 'Garden' },
  { title: 'Cykelverkstad', value: 'BikeWorkshop' },
]

function hasValidCoords() {
  return (
    typeof form.latitude === 'number' &&
    typeof form.longitude === 'number' &&
    Math.abs(form.latitude) > 0.0001 &&
    Math.abs(form.longitude) > 0.0001
  )
}

async function geocodFromAddress() {
  coordsHint.value = ''
  if (!form.address?.trim()) return
  geocoding.value = true
  try {
    const r = await api.geocodeSearch(form.address)
    form.latitude = r.latitude
    form.longitude = r.longitude
    coordsHint.value = 'Hittade: ' + (r.displayName?.slice(0, 80) || 'OK')
    message.value = ''
  } catch (e) {
    coordsHint.value = ''
    message.value = e.body?.error || 'Kunde inte hitta adressen. Lägg till "Göteborg" och försök igen, eller klicka på kartan.'
    messageType.value = 'error'
  } finally {
    geocoding.value = false
  }
}

async function handleSubmit() {
  const { valid: v } = await formRef.value.validate()
  if (!v) return
  if (!hasValidCoords()) {
    message.value = 'Sätt plats med "Hitta från adress" eller klicka på kartan.'
    messageType.value = 'warning'
    return
  }
  submitting.value = true
  message.value = ''
  try {
    await api.createLocation(form)
    message.value = 'Platsen är tillagd!'
    messageType.value = 'success'
    form.name = ''
    form.address = ''
    form.type = ''
    form.description = ''
    form.latitude = 0
    form.longitude = 0
    form.isFree = true
    form.tags = []
    coordsHint.value = ''
    setTimeout(() => router.push('/places'), 1500)
  } catch (e) {
    message.value = e.body?.error || e.message || 'Kunde inte lägga till plats'
    messageType.value = 'error'
  } finally {
    submitting.value = false
  }
}
</script>

<style scoped>
.add-place-page {
  max-width: 640px;
  padding-bottom: 2rem;
}

.page-title {
  font-size: 1.5rem;
  font-weight: 600;
  color: #1B5E20;
  margin: 0 0 0.25rem 0;
}

.page-subtitle {
  color: #666;
  margin-bottom: 1.5rem;
}

.form-card {
  background: white;
  border-radius: 16px;
  padding: 1.5rem;
  box-shadow: 0 4px 20px rgba(27, 94, 32, 0.08);
}

.section-label {
  font-size: 1rem;
  font-weight: 600;
  color: #1B5E20;
  margin: 0.5rem 0 0.25rem 0;
}
</style>
