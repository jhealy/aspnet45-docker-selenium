# meant to be run inside docker container
Invoke-WebRequest http://localhost -UseBasicParsing

.\seleniumtests\SeleniumDockerTest.exe http://localhost