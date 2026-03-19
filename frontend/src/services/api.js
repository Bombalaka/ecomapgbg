const baseUrl = import.meta.env.VITE_API_URL || '/api'

async function request(path, options = {}) {
  const url = `${baseUrl}${path}`
  const res = await fetch(url, {
    headers: {
      'Content-Type': 'application/json',
      ...options.headers,
    },
    ...options,
  })
  if (!res.ok) {
    const err = new Error(res.statusText)
    err.status = res.status
    err.body = await res.json().catch(() => ({}))
    throw err
  }
  return res.json()
}

export const api = {
  getLocations() {
    return request('/locations')
  },
  getLocationById(id) {
    return request(`/locations/${id}`)
  },
  createLocation(data) {
    return request('/locations', {
      method: 'POST',
      body: JSON.stringify(data),
    })
  },
  searchNearby(lat, lng, radiusKm = 5) {
    return request(`/locations/search?latitude=${lat}&longitude=${lng}&radiusKm=${radiusKm}`)
  },

  /** Address → coordinates via backend (OpenStreetMap Nominatim) */
  geocodeSearch(query) {
    const q = encodeURIComponent(query.trim())
    return request(`/geocode/search?q=${q}`)
  },
}
