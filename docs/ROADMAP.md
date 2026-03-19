# EcoMapGbg – roadmap & plan

## Now (current focus)

- Map + list of eco places (Leaflet/OSM; optional Mapbox)
- Add place with geocoding + map picker
- MongoDB + ASP.NET Core API
- Static info pages: **Om** (`/about`), **Hjälp** (`/help`)

## Near term (implemented)

| Idea | Status |
|------|--------|
| **Filter / search** | Done – `/places`: category chips + search (name, address, description) |
| **Better empty states** | Done – home alert when 0 places; places page empty + filtered-empty |
| **Events / nyheter** | Done – static `/events` (mall för framtida datum) |
| **Attribution footer** | Done – OSM, Nominatim, GitHub; Mapbox nämns om token finns |

## Later (v3+)

| Idea | Notes |
|------|--------|
| **“Eco-resa” / gamification** | Visits, badges, progress – needs **users** (auth) and **stored visits** in DB; was removed from MVP |
| **User accounts** | Login to save favourites or mark “visited” |
| **Moderation** | Approve new places before public map |
| **Geocoding queue / cache** | Reduce Nominatim calls, respect rate limits at scale |

## Removed from MVP

- **Journey page** (`/journey`): placeholder only; replaced by Om + Hjälp until real user data exists.

---

Update this file as priorities change.
