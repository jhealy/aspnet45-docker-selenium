Write-Host "win10 - step02 - build out aspnet.windows"

#Set-PSDebug -Trace 0
Set-PSDebug -Off

docker build -f win10.aspnet.windows.dockerfile . -t dotnet/framework/aspnet-windows:4.8

docker images --all
