<template>
  <div class="page">
    <h1 class="title">Händelser &amp; nyheter</h1>
    <p class="lead">
      Här kan framtida workshops, loppisar, reparationscaféer och stadens initiativ listas.
      Just nu är sidan en statisk mall – ni fyller på när det finns riktiga datum.
    </p>

    <v-card class="event-card mb-4" rounded="xl" elevation="0">
      <v-card-title class="event-title">Reparationscafé – exempel</v-card-title>
      <v-card-subtitle>Kommer snart · Göteborg</v-card-subtitle>
      <v-card-text>
        Tips: lägg in egna event i koden eller koppla till en enkel admin senare.
        Under tiden: följ lokala bibliotek och Miljöbron för riktiga datum.
      </v-card-text>
    </v-card>

    <v-card class="event-card mb-4" rounded="xl" elevation="0">
      <v-card-title class="event-title">Byt dag – klädbyt</v-card-title>
      <v-card-subtitle>Återkommande · kolla Facebook / kommunens kalender</v-card-subtitle>
      <v-card-text>
        Vill du synas här? Kontakta projektet via GitHub README.
      </v-card-text>
    </v-card>

    <div class="cta">
      <v-btn to="/places" color="primary" variant="tonal" rounded="lg">Till platserna</v-btn>
      <v-btn to="/places/add" color="primary" rounded="lg" class="ml-2">Lägg till plats</v-btn>
    </div>
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
  line-height: 1.6;
  margin-bottom: 1.5rem;
}

.event-card {
  background: #fff;
  box-shadow: 0 4px 20px rgba(27, 94, 32, 0.08) !important;
}

.event-title {
  font-size: 1.1rem;
  font-weight: 600;
  color: #1b5e20;
}

.cta {
  margin-top: 1.5rem;
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
}
</style>
