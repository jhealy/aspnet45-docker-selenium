docker container rm -f $(docker ps -a -q)
docker image rm aspnet48
docker image rm aspnet48tests -f
docker image prune -f