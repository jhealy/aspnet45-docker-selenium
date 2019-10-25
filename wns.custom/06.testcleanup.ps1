#Set-PSDebug -Trace 1
Set-PSDebug -Off

docker container rm wincustomaspnet48testsrun -f
docker image rm wincustomaspnet48tests -f
