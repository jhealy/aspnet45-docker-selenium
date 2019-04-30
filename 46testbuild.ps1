Write-Host "Add seleniumtests and support files to aspnet48 container"

Set-PSDebug -Trace 1

docker build -f 46test.dockerfile -t 46tests . 

docker run -d --name 46testsrun -p 5000:80 46tests
docker container ls 

Invoke-WebRequest http:\\localhost:5000 -UseBasicParsing

Write-Host "!!! here we should actually silent install FireFox and Chrome using exe if we dont get it working w choco"

docker exec -it 46testsrun powershell