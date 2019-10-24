# SELENIUM TESTS FOR .NET FRAMEWORK 4.X IN A WINDOWS SERVER CONTAINER

This document shows you how to build out a container that:

* Contains a Mvc45 web application
* Contains a selenium test executing against the Mvc45 web application
* SELENIUM CRASHES in a container using any version of server and .NET Framework 4.x

## PRE-REQUISITES

* Windows 10 v.latest.  
* Install Docker for Windows into Windows 10. Other platforms may work but are not tested.  https://docs.docker.com/docker-for-windows/
* Git for Windows and some knowledge of how to clone a repo down.  https://git-scm.com/download/win

## CLONE DOWN THIS REPO

* Open a command prompt and navigate to where you with this repo to be copied.
* Issue the following command to clone the repo locally

```powershell
PS > git clone https://github.com/jhealy/aspnet45-docker-selenium.git
```

## MAKE SURE DOCKER IS SET TO WINDOWS CONTAINERS

* Locate docker in the task bar of Win10 and click on it
* If Docker has a menu entry that says "Switch to Linux containers..." you are fine.  This means you are in Windows container mode.
* If Docker has a menu entry that says "Switch to Windows containers..." click it.  You are in Linux container mode and must switch to Windows containers.  Wait for docker to restart.

![docker popup](images/docker-popup.jpg)

* Open a command prompt and issue the following command.  You should see info about the current state of docker.  If the command doesn't run something has happened to docker.

```powershell
PS > docker system info
```

![docker popup](images/docker-system-info.jpg)

## BUILD OUR CONTAINERS

Docker uses [dockerfiles](https://docs.docker.com/engine/reference/builder) to specify how to build out and label containers.  In this repo are a series of powershell scripts which execute a series of dockerfiles to build out the containers.  

Note running the docker builds, especially the first dockerfiles for the base containers can take a bit. The main time activity is downloading the base docker image. Make sure you have a fast network connection and a book to read.

* Open a powershell window and navigate to the **wns.net4-8** directory in the repo.
* Run the command below.  Note this command can take up to 20 minutes to run so get coffee. This command does not need to run for each new test build.  We are using it to build out .NET Framework Windows Server images. The dockerfile also installs firefox and chrome into the server image.

```powershell
PS > .\buildbasecontainer.ps1
```

![docker popup](images/docker-build-base-container.jpg)

* Run the following command.  The website and selenium test are moved into our container.  You won't ordinarily see the screen below but it shows the copy operations begin run by the dockerfile.

```powershell
PS > .\buildtestcontainer.ps1
```

![docker popup](images/interactive-terminal.jpg)

* Note we are now in an interactive shell inside our container.  Run the following command to run the test.  Test results are routed to the screen, but could be dumped to logs for parsing, or any other options you can code.

```powershell
PS > .\1.ps1
```

In the screenshot below, you can see the CheckWebElements fails.

![failed test run](images/selenium-test-fail.jpg)

To exit the container and return to the shell of your local machine / vm type "exit" in the container and hit enter.

```powershell
PS > exit
```

Carpe docker, now go try the Win10 test that works!
