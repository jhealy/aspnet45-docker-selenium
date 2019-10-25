Write-Host "Windows Custom Build - STEP 03 - build out base container"

#Set-PSDebug -Trace 0
Set-PSDebug -Off

docker build -f 03.buildbasecontainer.dockerfile . -t wincustombasecontainer

docker images --all
