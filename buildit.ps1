Write-Host "Build out our windows docker aspnet 45 selenium container"

Set-PSDebug -Trace 1

docker build -f dockerfile -t aspnet45 . 
docker images --all
docker run -d --name aspnet45run -p 5000:80 aspnet45
docker container ls 

Invoke-WebRequest http:\\localhost:5000 -UseBasicParsing

Write-Host "!!! here we should actually silent install FireFox and Chrome using exe if we dont get it working w choco"

docker exec -it aspnet45run powershell 