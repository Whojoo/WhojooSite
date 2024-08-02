FROM node:20-alpine AS build
WORKDIR /app/src
COPY ["front-end/package*.json", "./"]
RUN npm ci
COPY ["front-end/", "./"]
RUN npm run build front-end

FROM node:20-alpine
WORKDIR /usr/app
COPY --from=build /app/src/dist/front-end ./
CMD node server/server.mjs
EXPOSE 4000
