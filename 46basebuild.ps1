Write-Host "Build out our windows docker aspnet 4.6.2 selenium container named _aspnet46_"

Set-PSDebug -Trace 1

docker build -f 46base.dockerfile -t aspnet46 . 
docker images --all
