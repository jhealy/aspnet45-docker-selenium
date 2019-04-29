Write-Host "Build out our windows docker aspnet48 selenium container named _aspnet48_"

Set-PSDebug -Trace 1

docker build -f buildbasecontainer.dockerfile -t aspnet48 . 
docker images --all
