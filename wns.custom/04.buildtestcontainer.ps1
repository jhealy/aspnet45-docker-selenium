Write-Host "Windows Custom Build  - STEP 04 - Add seleniumtests and support files to win10 aspnet48 container"

Set-PSDebug -Trace 0
# Set-PSDebug -Off

docker build -f 04.buildtestcontainer.dockerfile -t wincustomaspnet48tests . 

docker run -d --name wincustomaspnet48testsrun -p 5000:80 wincustomaspnet48tests

docker container ls 

# Invoke-WebRequest http:\\localhost:5000 -UseBasicParsing

Write-Output "enter test container: docker exec -it wincustomaspnet48testsrun powershell"

docker exec -it wincustomaspnet48testsrun powershell