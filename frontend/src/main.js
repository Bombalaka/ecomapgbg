import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import './assets/global.css'
import 'vuetify/styles'
import '@mdi/font/css/materialdesignicons.css'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const vuetify = createVuetify({
  components,
  directives,
  theme: {
    defaultTheme: 'eco',
    themes: {
      eco: {
        dark: false,
        colors: {
          primary: '#1B5E20',
          secondary: '#2E7D32',
          accent: '#66BB6A',
          surface: '#F8FDF8',
          background: '#F1F8F1',
          'eco-warm': '#8D6E63',
          'eco-mint': '#A5D6A7',
        },
      },
    },
  },
})

createApp(App).use(vuetify).use(router).mount('#app')
