Write-Host "win10 - step01 - build out dotnet.windows"

Set-PSDebug -Trace 0

docker build -f win10.dotnet.windows.dockerfile . -t dotnet/framework/runtime:4.8-windows-ltsc2019

docker images --all
