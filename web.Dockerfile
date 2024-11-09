# Base image
FROM nginx:latest AS base

# Stage 1: Build
FROM node:20-alpine AS build
WORKDIR /app/src
COPY ["frontend/package*.json", "./"]
RUN npm ci
COPY ["frontend/", "./"]
RUN npm run build WhojooSite.Angular

# Stage 2: Run
FROM base AS runtime
COPY ./frontend/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/src/dist/whojoo-site.angular/browser /usr/share/nginx/html

EXPOSE 80
