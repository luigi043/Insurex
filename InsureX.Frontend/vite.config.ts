import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    tailwindcss(),  // Tailwind v4 requires this Vite plugin (not PostCSS)
    react(),
  ],
  server: {
    port: 5173,
    proxy: {
      // Proxy /api calls to the .NET API during local dev
      '/api': {
        target: 'http://localhost:5062',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})