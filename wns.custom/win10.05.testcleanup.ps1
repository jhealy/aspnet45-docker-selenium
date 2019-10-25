#Set-PSDebug -Trace 1
Set-PSDebug -Off

docker container rm w10aspnet48testsrun -f
docker image rm w10aspnet48tests -f
