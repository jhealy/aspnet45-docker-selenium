Write-Host "win10 - step03 - build out base container"

Set-PSDebug -Trace 1

docker build -f win10.buildbasecontainer.dockerfile . -t w10basecontainer

docker images --all
