FROM node:20-alpine AS build
WORKDIR /app/src
COPY package*.json ./
RUN npm ci
COPY . ./
RUN npm run build front-end

FROM node:20-alpine
WORKDIR /usr/app
COPY --from=build /app/src/dist/front-end ./
CMD node server/server.mjs
EXPOSE 4000
