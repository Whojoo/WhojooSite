FROM node:20-alpine AS build
WORKDIR /app/src
COPY ["frontend/package*.json", "./"]
RUN npm ci
COPY ["frontend/", "./"]
RUN npm run build frontend

FROM node:20-alpine
WORKDIR /usr/app
COPY --from=build /app/src/dist/frontend ./
CMD node server/server.mjs
EXPOSE 4000
