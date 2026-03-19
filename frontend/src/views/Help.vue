<template>
  <div class="page">
    <h1 class="title">Hjälp</h1>
    <p class="lead">Så här använder du EcoMapGbg.</p>

    <v-card class="block" rounded="xl" elevation="0">
      <v-card-title class="block-title">Lägga till en plats</v-card-title>
      <v-card-text class="text-body-1">
        <ol class="steps">
          <li>Fyll i <strong>namn</strong> och <strong>adress</strong> (gärna med stad, t.ex. Göteborg).</li>
          <li>Klicka <strong>Hitta från adress</strong> för att sätta koordinater automatiskt.</li>
          <li>Du kan också <strong>klicka på kartan</strong> för att finjustera platsen.</li>
          <li>Välj kategori och eventuell beskrivning, sedan <strong>Skicka</strong>.</li>
        </ol>
      </v-card-text>
    </v-card>

    <v-card class="block" rounded="xl" elevation="0">
      <v-card-title class="block-title">Geocoding (adress → karta)</v-card-title>
      <v-card-text class="text-body-1">
        <p>
          Sökningen går via OpenStreetMap <strong>Nominatim</strong>. Det är gratis men har begränsningar:
          högst ungefär en sökning i sekunden. Om inget hittas, prova tydligare adress eller sätt punkten manuellt på kartan.
        </p>
      </v-card-text>
    </v-card>

    <v-card class="block" rounded="xl" elevation="0">
      <v-card-title class="block-title">Kartan</v-card-title>
      <v-card-text class="text-body-1">
        <p>
          Utan Mapbox-token används <strong>Leaflet + OpenStreetMap</strong> (gratis). Med token i
          <code>frontend/.env</code> kan du använda Mapbox-stilar.
        </p>
        <p class="mb-0">
          Klicka på en plats i kartan för detaljer. Klicka på ett <strong>kort</strong> under listan för mer information.
        </p>
      </v-card-text>
    </v-card>

    <v-card class="block" rounded="xl" elevation="0">
      <v-card-title class="block-title">Kör appen lokalt</v-card-title>
      <v-card-text class="text-body-1">
        <p class="mb-0">
          Starta MongoDB (t.ex. <code>docker-compose up -d mongodb</code>), kör API med
          <code>dotnet run --project EcoMapGbg</code> och frontend med <code>npm run dev</code> i mappen
          <code>frontend</code>. Se README för mer.
        </p>
      </v-card-text>
    </v-card>

    <p class="foot">
      <router-link to="/about">Om projektet</router-link>
      ·
      <router-link to="/places">Platser</router-link>
    </p>
  </div>
</template>

<style scoped>
.page {
  max-width: 640px;
  padding-bottom: 2rem;
}

.title {
  font-size: 1.75rem;
  font-weight: 700;
  color: #1b5e20;
  margin: 0 0 0.75rem 0;
}

.lead {
  font-size: 1.05rem;
  color: #444;
  margin-bottom: 1.5rem;
}

.block {
  background: #fff;
  box-shadow: 0 4px 20px rgba(27, 94, 32, 0.08) !important;
  margin-bottom: 1rem;
}

.block-title {
  font-size: 1.1rem;
  font-weight: 600;
  color: #1b5e20;
}

.steps {
  margin: 0;
  padding-left: 1.25rem;
}

.steps li {
  margin-bottom: 0.5rem;
}

code {
  background: #f1f8f1;
  padding: 0.1rem 0.35rem;
  border-radius: 4px;
  font-size: 0.9em;
}

.foot {
  margin-top: 1.5rem;
  font-size: 0.95rem;
}
</style>
