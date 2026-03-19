import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vuetify from 'vite-plugin-vuetify'

export default defineConfig({
  plugins: [
    vue(),
    vuetify({ autoImport: true }),
  ],
  base: '/',
  build: {
    outDir: 'dist',
  },
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5202',
        changeOrigin: true,
      },
      '/swagger': {
        target: 'http://localhost:5202',
        changeOrigin: true,
      },
      '/maphub': {
        target: 'http://localhost:5202',
        ws: true,
      },
    },
  },
})
