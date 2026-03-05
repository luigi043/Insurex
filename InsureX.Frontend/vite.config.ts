import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      // Proxy all /api requests to the .NET backend during local dev
      // This avoids CORS issues when running: npm run dev
      '/api': {
        target: 'http://localhost:5062',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})