SERVER=dev
API_GATEWAY=dev
CONFIG=Debug

run:
	docker compose up -d

stop:
	docker compose stop
	
logs:
	docker compose logs -f

logs-server:
	docker compose logs -f server

rebuild: build-server build-api-gateway run

rebuild-server: build-server
	docker compose up -d server

rebuild-api-gateway: build-api-gateway
	docker compose up -d api-gateway
	
build-server:
	docker build --file server.Dockerfile --build-arg BUILD_CONFIGURATION=${CONFIG} --tag whojoo-site/server:${SERVER} .
	
build-api-gateway:
	docker build --file api-gateway.Dockerfile --build-arg BUILD_CONFIGURATION=${CONFIG} --tag whojoo-site/api-gateway:${API_GATEWAY} .
