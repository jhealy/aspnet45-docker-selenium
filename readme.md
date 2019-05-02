# DOCKER WITH SELENIUM AND ASP.NET 4.5 MVC

What am I trying to do?

This is my personal quest to have a  docker image that

* Runs an ASP.NET MVC 4.5 web app
* Has selenium driver via a c# console exe testing the code

Current status - when test is run in regular Windows 10 or Windows 2016 it runs fine.  When the test is run in a Windows docker container it blows out with "***session deleted because of page crash***".

I turned on verbose debugging for selenium and trapped out logfiles.  I have two logfiles.  ["goodrun_log.txt"](logs/goodrun_log.txt) is from a Windows 10 successful test.  ["docker_log.txt"](logs/docker_log.txt) is the log from a failed run inside a container.

About line 473 we can see the docker run fail.  Up to that point the log file is exactly the same as the good run.  Then boom.  So what are we missing that makes the docker container fail at that point?  

## SEEMS SIMILAR TO

* Docker issue (linux) on aug 11 2015 - /dev/shm sizing - https://github.com/elgalu/docker-selenium/issues/20 by kkochubey1
* Docker issue (linux) march 2018 - https://github.com/pranavgore09/fabric8-planner/pull/3

## THINGS I TRIED

* SHM mode. Command runs but did not resolve issue 

```powershell
docker run -d --name aspnet48testsrun --shm-size="1g" -p 5000:80 aspnet48testsd
```

* Infinite memory

```
docker run -d --name aspnet48testsrun -m inf --memory-swap inf -p 5000:80 aspnet48tests
```

## DOCKER RUN FAIL

<pre>
[1556732925.450][DEBUG]: DevTools WebSocket Event: DOM.documentUpdated 7FCEC12C5F4ADEA352BBA3DF3AF6075D {

}
[1556732925.450][DEBUG]: DevTools WebSocket Command: DOM.getDocument (id=15) 7FCEC12C5F4ADEA352BBA3DF3AF6075D {

}
[1556732925.451][DEBUG]: DevTools WebSocket Response: Runtime.evaluate (id=14) 7FCEC12C5F4ADEA352BBA3DF3AF6075D {
   "result": {
      "type": "string",
      "value": "http://localhost/"
   }
}
[1556732925.531][DEBUG]: DevTools WebSocket Event: Inspector.targetCrashed 7FCEC12C5F4ADEA352BBA3DF3AF6075D {

}
[1556732925.532][INFO]: Waiting for pending navigations...
[1556732925.532][DEBUG]: DevTools WebSocket Command: Runtime.evaluate (id=16) 7FCEC12C5F4ADEA352BBA3DF3AF6075D {
   "expression": "1"
}
[1556732925.532][INFO]: Done waiting for pending navigations. Status: unknown error: cannot determine loading status
from tab crashed
[1556732925.552][INFO]: [464b2b630c39434969f9b90e11b7aa37] RESPONSE Navigate ERROR unknown error: session deleted because of page crash
from unknown error: cannot determine loading status
from tab crashed
  (Session info: headless chrome=74.0.3729.108)
[1556732925.552][DEBUG]: Log type 'driver' lost 0 entries on destruction
[1556732925.552][DEBUG]: Log type 'browser' lost 0 entries on destruction
</pre>

## WIN10 RUN GOOD

<pre>
[1556733552.098][DEBUG]: DevTools WebSocket Event: DOM.documentUpdated 193B5CE9ACD5F7CE56919120C68276A7 {

}
[1556733552.098][DEBUG]: DevTools WebSocket Command: DOM.getDocument (id=15) 193B5CE9ACD5F7CE56919120C68276A7 {

}
[1556733552.104][DEBUG]: DevTools WebSocket Response: Runtime.evaluate (id=14) 193B5CE9ACD5F7CE56919120C68276A7 {
   "result": {
      "type": "string",
      "value": "http://localhost:29657/"
   }
}
[1556733552.104][DEBUG]: DevTools WebSocket Response: DOM.getDocument (id=15) 193B5CE9ACD5F7CE56919120C68276A7 {
   "root": {
      "backendNodeId": 6,
      "baseURL": "http://localhost:29657/",
      "childNodeCount": 1,
      "children": [ {
         "attributes": [  ],
         "backendNodeId": 7,
         "childNodeCount": 2,
... lots more ...
</pre>

## SESSION DELETED BECAUSE OF PAGE CRASH

<pre>
PS C:\seleniumtests> .\SeleniumDockerTest.exe http://localhost
[chrome options:] =[--headless --no-sandbox --disable-gpu]
Starting ChromeDriver 74.0.3729.6 (255758eccf3d244491b8a1317aa76e1ce10d57e9-refs/branch-heads/3729@{#29}) on port 49160
Only local connections are allowed.
Please protect ports used by ChromeDriver and related test frameworks to prevent access by malicious code.
[0501/120039.381:ERROR:network_change_notifier_win.cc(156)] WSALookupServiceBegin failed with: 0
[0501/120039.428:ERROR:audio_device_listener_win.cc(46)] RegisterEndpointNotificationCallback failed: 80070424

DevTools listening on ws://127.0.0.1:49163/devtools/browser/f33a8cd9-6411-46f5-a9ab-d69901cd53c1
[0501/120039.772:ERROR:network_change_notifier_win.cc(156)] WSALookupServiceBegin failed with: 0
[exception caught] =[OpenQA.Selenium.WebDriverException: unknown error: session deleted because of page crash
from unknown error: cannot determine loading status
from tab crashed
  (Session info: headless chrome=74.0.3729.108)
  (Driver info: chromedriver=74.0.3729.6 (255758eccf3d244491b8a1317aa76e1ce10d57e9-refs/branch-heads/3729@{#29}),platform=Windows NT 10.0.17763 x86_64)
   at OpenQA.Selenium.Remote.RemoteWebDriver.UnpackAndThrowOnError(Response errorResponse)
   at OpenQA.Selenium.Remote.RemoteWebDriver.Execute(String driverCommandToExecute, Dictionary`2 parameters)
   at OpenQA.Selenium.Remote.RemoteWebDriver.set_Url(String value)
   at OpenQA.Selenium.Remote.RemoteNavigator.GoToUrl(String url)
   at SeleniumDockerTest.Program.DoChromeTests() in C:\dev\docker-selenium-aspnet45.git\SeleniumDockerTest\Program.cs:line 60]
</pre>

## TRY IT YOURSELF

There is a docker image with Windows, IIS, Chrome, FF and some tests at https://cloud.docker.com/repository/docker/jhealy62/devfish .

Pull it down the repo and provision it

* docker pull jhealy62/devfish
* docker run -d --name aspnettest -p 5000:80 jhealy62/devfish

Powershell into the container

* docker exec -it aspnettest powershell

Inside the docker container, see the web server working

* curl http://localhost -UseBasicParsing

See the seleniumtest failing:

* cd \
* cd \seleniumtests
* .\SeleniumDockerTests.exe http://localhost

Cry with me!

## NEXT STEPS

* POSTED ISSUE - Aspnet docker github - https://github.com/Microsoft/aspnet-docker/issues/181

## WHAT HAPPENED TO THE PAGE TIMEOUT ISSUE?

WebDriver Timeout error resolved -Occurs with either FireFox or Chrome tests inside docker container.  FIX (requires both items below):

* Install websocket's into the docker container.  Excerpt from dockerfile:

```powershell
RUN powershell -Command Add-WindowsFeature Web-WebSockets
```

* Pass a very interesting set of options to the chromedriver.

```C#
option.AddArguments( "--headless","--disable-gpu", "--no-sandbox" );
```