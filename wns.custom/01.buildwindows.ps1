Write-Host "Windows Custom Build - STEP 01 - build out dotnet.windows"

Set-PSDebug -Off
# Set-PSDebug -Trace 0

docker build -f 01.dotnet.windows.dockerfile . -t dotnet/framework/runtime:4.8-windows-ltsc2019

docker images --all
