# Build stage
FROM node:18-alpine AS build
WORKDIR /app

# Accept build-time API URL (injected by docker-compose)
ARG VITE_API_URL=/api
ENV VITE_API_URL=$VITE_API_URL

# Copy package files and install deps
COPY package*.json ./
RUN npm install

# Copy source and build
COPY . .
RUN npm run build

# Production stage – serve with nginx
FROM nginx:stable-alpine
# FIX: was "COPY --from:build" (colon is wrong) → must be "COPY --from=build"
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]