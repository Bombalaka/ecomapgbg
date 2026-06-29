# EcoMapGbg

## Connecting Gothenburg with reuse spots, second-hand shops, and repair hubs for a more circular city.

**EcoMapGbg** (Återbrukskartan) is a location-based web app for promoting reuse, sharing, and circular consumption in Gothenburg.

**Roadmap:** see [`ROADMAP.md`](ROADMAP.md) in the repo root (what’s done, near-term, v3+ ideas).

**Deploy (Railway):** step-by-step in [`DEPLOY-RAILWAY.md`](DEPLOY-RAILWAY.md).

---

## Project Overview

EcoMapGbg helps people in Gothenburg discover and share places where items can be reused instead of thrown away – such as second-hand shops, repair stations, swap shelves, and recycling centers.

---

## Tech Stack (v2)

| Area       | Technology                    |
|------------|-------------------------------|
| Frontend   | Vue 3, Vuetify, Mapbox GL JS   |
| Backend    | ASP.NET Core 9, C#             |
| Database   | MongoDB, Docker               |
| Deployment | Docker Compose                |
| API Docs   | Swagger / OpenAPI             |

---

## Project Structure

```
ecomapgbg/
├── EcoMapGbg/              # ASP.NET Core API
│   ├── Controllers/
│   ├── Data/
│   ├── Models/
│   ├── Services/
│   └── wwwroot/            # Vue build output (Docker)
├── frontend/               # Vue 3 + Vuetify SPA
│   ├── src/
│   │   ├── components/
│   │   ├── views/
│   │   ├── services/
│   │   └── router/
│   └── package.json
├── ROADMAP.md              # Plan: near-term + future (e.g. eco-resa v3)
├── frontend/public/
│   ├── logo-ecomapgbg.svg  # Custom brand mark (map + sprout)
│   └── favicon.svg
├── docker-compose.yml
├── Dockerfile
└── .env
```

**App routes:** `/` (landing / video intro — **only first visit**; then redirects to `/home`). Add `/?replay=1` or use the link on Home to watch the intro again. `/home` (dashboard), `/places`, `/places/add`, `/events` (Händelser), `/about`, `/help`.

---

## Environment Variables

### Root `.env` (MongoDB + Docker)

| Variable        | Description              | Example  |
|----------------|--------------------------|----------|
| MONGO_USERNAME | MongoDB admin username   | root     |
| MONGO_PASSWORD | MongoDB admin password   | example  |
| MONGO_DB       | Database name            | ecomapgbg |

### Frontend `frontend/.env` (optional, for local dev)

| Variable                 | Description                          | Example                    |
|--------------------------|--------------------------------------|----------------------------|
| VITE_MAPBOX_TOKEN        | Mapbox access token (get at mapbox.com) | pk.eyJ1...                 |
| VITE_LANDING_VIDEO_URL   | Optional MP4 loop for `/` landing (muted background) | `/videos/landing.mp4`      |
| VITE_API_URL             | API base URL (default: /api for same-origin) | http://localhost:5202/api |

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [Node.js 20+](https://nodejs.org/)
- [Docker](https://www.docker.com/) (for MongoDB and full-stack run)

### Local Development

**1. Database**

- **MongoDB Atlas:** add `ConnectionStrings__MongoDB` to the repo-root `.env` → skip local Mongo.
- **Local Mongo in Docker:** run:
  ```bash
  docker compose --profile local-mongo up -d mongodb mongo-express
  ```

**2. Run the API**

```bash
dotnet run --project EcoMapGbg
```

API: http://localhost:5202  
Swagger: http://localhost:5202/swagger

**3. Run the frontend**

```bash
cd frontend
npm install
npm run dev
```

Frontend: http://localhost:5173 (proxies /api to the API)

**4. Mapbox (optional)**

Create a free token at [mapbox.com](https://mapbox.com) and add to `frontend/.env`:

```
VITE_MAPBOX_TOKEN=pk.eyJ1...
```

Without a token, maps use **Leaflet + OpenStreetMap** (free).

**Add place:** Enter an address and click **Hitta från adress**, or click the map to set coordinates. The backend calls [Nominatim](https://nominatim.openstreetmap.org/) (max ~1 request/second per their policy).

### Full Stack with Docker

**With MongoDB Atlas** (`.env` has `ConnectionStrings__MongoDB`):

```bash
docker compose up --build
```

Only the app container runs; it talks to Atlas.

**With local Mongo** (comment out `ConnectionStrings__MongoDB` in `.env` so the compose fallback is used):

```bash
docker compose --profile local-mongo up --build
```

Optional: `VITE_MAPBOX_TOKEN` in `.env` for the Docker build (Mapbox in the baked SPA).

App: http://localhost:5000

---

## API Endpoints

| Method | Path                    | Description          |
|--------|-------------------------|----------------------|
| GET    | /api/locations          | Get all locations    |
| GET    | /api/locations/{id}     | Get location by ID   |
| POST   | /api/locations          | Create location      |
| GET    | /api/locations/search   | Search nearby        |
| GET    | /api/locations/health   | Health check         |
| GET    | /api/geocode/search?q=  | Address → lat/lon (Nominatim, Sweden bias) |

---

## Contributors

- [Nor](https://github.com/NorAjami) – Project Lead
- [Yotaka](https://github.com/Yotaka88) – Project Lead

---

## Security & Privacy

- MongoDB connection uses environment variables.
- Inputs are validated client- and server-side.
- No user tracking or ads.

## DEMO 

https://github.com/user-attachments/assets/7942f4e9-1284-4dac-936d-12cc4543daa4


## License

MIT License – feel free to reuse and contribute.
