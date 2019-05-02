Write-Host "Add seleniumtests and support files to aspnet48 container"

Set-PSDebug -Trace 1

docker build -f buildtestcontainer.dockerfile -t aspnet48tests . 

docker run -d --name aspnet48testsrun -m inf --memory-swap inf -p 5000:80 aspnet48tests
docker container ls 

# Invoke-WebRequest http:\\localhost:5000 -UseBasicParsing

docker exec -it aspnet48testsrun powershell