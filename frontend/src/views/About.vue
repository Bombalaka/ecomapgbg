<template>
  <div class="page">
    <h1 class="title">Om EcoMapGbg</h1>
    <p class="lead">
      <strong>Återbrukskartan</strong> – en karta över platser i Göteborg där du kan återbruka, laga och handla hållbart.
    </p>

    <v-card class="block" rounded="xl" elevation="0">
      <v-card-title class="block-title">Vad gör appen?</v-card-title>
      <v-card-text class="text-body-1">
        <ul class="list">
          <li>Utforska <router-link to="/places">eco-platser</router-link> på karta och i lista.</li>
          <li><router-link to="/places/add">Lägg till</router-link> second-hand, reparation, återvinning med mera.</li>
          <li>All data sparas så fler kan hitta rätt plats i staden.</li>
        </ul>
      </v-card-text>
    </v-card>

    <v-card class="block" rounded="xl" elevation="0">
      <v-card-title class="block-title">Teknik &amp; tack</v-card-title>
      <v-card-text class="text-body-1">
        <p>
          Kartor använder <strong>OpenStreetMap</strong> (gratis kartdata). Adresser kan geokodas via
          <strong>Nominatim</strong> (OpenStreetMap). Läs mer under <router-link to="/help">Hjälp</router-link>.
        </p>
        <p class="mb-0">
          Byggd med Vue, Vuetify och ASP.NET Core med MongoDB.
        </p>
      </v-card-text>
    </v-card>

    <v-card class="block" rounded="xl" elevation="0">
      <v-card-title class="block-title">Bidra</v-card-title>
      <v-card-text class="text-body-1">
        <p class="mb-0">
          Tips, fel eller idéer? Kontakta projektet via er GitHub eller README i repot.
        </p>
      </v-card-text>
    </v-card>
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
  line-height: 1.6;
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

.list {
  margin: 0;
  padding-left: 1.25rem;
}

.list li {
  margin-bottom: 0.5rem;
}
</style>
