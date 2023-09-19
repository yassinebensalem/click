.ONESHELL: # Applies to every targets in the file!
.PHONY:	 status stop-dev stop-prod start-prod start-dev 

status:
	docker ps

stop-dev:
	docker-compose stop web-dev

stop-prod:
	docker-compose stop web-prod

start-prod: 
	docker-compose up -d web-prod

start-dev:
	docker-compose up -d web-dev
