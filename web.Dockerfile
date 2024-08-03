FROM node:20-alpine AS build
WORKDIR /app/src
COPY ["frontend/package*.json", "./"]
RUN npm ci
COPY ["frontend/", "./"]
RUN npm run build frontend

FROM node:20-alpine AS runtime
WORKDIR /usr/app
COPY --from=build /app/src/dist/frontend ./
EXPOSE 4000
ENTRYPOINT [ "node", "server/server.mjs" ]
