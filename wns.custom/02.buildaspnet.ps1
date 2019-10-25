Write-Host "Windows Custom Build - STEP 02- build out aspnet.windows"

#Set-PSDebug -Trace 0
Set-PSDebug -Off

docker build -f 02.aspnet.windows.dockerfile . -t dotnet/framework/aspnet-windows:4.8

docker images --all
