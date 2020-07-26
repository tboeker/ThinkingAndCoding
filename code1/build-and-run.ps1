.\build.ps1

docker-compose -f docker-compose.yml -f docker-compose-apps.yml down --remove-orphans
docker-compose -f docker-compose.yml -f docker-compose-apps.yml up