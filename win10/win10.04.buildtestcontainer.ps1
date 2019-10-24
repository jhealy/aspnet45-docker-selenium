Write-Host "win10 - Add seleniumtests and support files to win10 aspnet48 container"

Set-PSDebug -Trace 0

docker build -f win10.buildtestcontainer.dockerfile -t w10aspnet48tests . 

docker run -d --name w10aspnet48testsrun -p 5000:80 w10aspnet48tests

docker container ls 

# Invoke-WebRequest http:\\localhost:5000 -UseBasicParsing

# docker exec -it w10aspnet48testsrun powershell