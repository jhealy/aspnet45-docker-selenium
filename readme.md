# DOCKER WITH SELENIUM AND ASP.NET 4.5 MVC

* Runs an ASP.NET MVC 4.5 web app
* Has selenium driver via a c# console exe testing the code successfully

## ISSUE AND RESOLUTION

* When the Selenium test is run in Windows 10 or Windows 2016 (on-the-box or VM) it runs fine.  
* When the Selenium test is run in a Windows Server docker container, the test fails with "***session deleted because of page crash***".
* When the Selenium test is run in a Windows 10 container in docker, the test executes successfully.

Findings

* You cannot run a Selenium test in a Windows Server container at this point.  Prove me wrong please.
* You CAN run a Selenium test in a Windows 10 container.

## WIN10 FOR THE WIN

Windows 10 containers are able to run Selenium tests.  See the instructions at [how to build a Selenium / .NET Framework / Windows 10 container](win10/buildme-win10.md) to get going.

### SEEMS SIMILAR TO

* Docker issue (linux) on aug 11 2015 - /dev/shm sizing - https://github.com/elgalu/docker-selenium/issues/20 by kkochubey1
* Docker issue (linux) march 2018 - https://github.com/pranavgore09/fabric8-planner/pull/3
* ChromeDriver - https://github.com/rshf/chromedriver/issues/772
* Chromium bug (linux) - https://bugs.chromium.org/p/chromium/issues/detail?id=522853

### POSTED, BUT NO SOLUTIONS

I posted this around quite a bit.  Below are the publish points.  I need to go and post responses to each of the items identifying Win10 containers as a possible solution for the issue.

* Submitted to - Aspnet docker github - https://github.com/Microsoft/aspnet-docker/issues/181
* Submitted to chromedriver on 0506/19  - https://github.com/rshf/chromedriver/issues/825
* Posted to ASP.NET docker repo on 0430/19 - no action -  https://github.com/Microsoft/aspnet-docker/issues/181
* Posted to stackoverflow on 0502/19 - https://stackoverflow.com/questions/55959477/selenium-inside-windows-docker-container-fails-with-ff-chrome-session-deleted-b
* hitting some internal microsoft email lists and contacts.  still pending.
* Posted to Selenium:  https://github.com/SeleniumHQ/selenium/issues/7165 . Selenium has said it is not a Selenium issue via [@diego](https://github.com/diemol).  Microsoft support guy says its diff issues [@n777ty](https://github.com/n777ty)

> "They are both related to running Selenium inside a Windows docker image, and the comment I mentioned above still applies. Whereas this is something interesting, there is not really a bug caused by Selenium, if you are looking for help I pointed out resources to get it, you could also ask in StackOverflow, or join us in the IRC/Slack channel where the community can help you as well."

## WINDOWS SERVER CONTAINER EFFORTS

Significant effort was spent trying to get the container to run in a variety of Windows Server containers.  Some of the efforts are documented below.

* Verbose logging enabled for selenium and trapped out logfiles.  You willfind two logfiles to reference.  ["goodrun_log.txt"](logs/goodrun_log.txt) is from a Windows 10 successful test.  ["docker_log.txt"](logs/docker_log.txt) is the log from a failed run inside a container.  See https://www.chromium.org/developers/design-documents/network-stack/netlog for info on how to enable logs for chromium.

* About line 473 in ["docker_log.txt"](logs/docker_log.txt)  he docker run fails.  Up to that point the log file is exactly the same as the Win10 successful text execution, as referenced in line 473 of good run, as refed in ["goodrun_log.txt"](logs/goodrun_log.txt).  

* A variety of Chrome flags were tried.  For reference, see the MvcHelloWorld45 for commented out flags.  File Program.cs, line 68 and onward.  An example is below for --disable-dev-shm-usage. 

```c#
option.AddArgument("--disable-dev-shm-usage"); // https://github.com/elgalu/docker-selenium/issues/20#issuecomment-407101358
```

* Chromium driver retry pattern, not successful.  Reference https://github.com/electron/electron/issues/9369#issuecomment-312234465
* Docker SHM mode. Command runs but did not resolve issue.

```powershell
docker run -d --name aspnet48testsrun --shm-size="1g" -p 5000:80 aspnet48testsd
```

* Docker memory allocation, unsuccessful.

```
docker run -d --name aspnet48testsrun -m inf --memory-swap inf -p 5000:80 aspnet48tests
docker run -d --name aspnet48testsrun -m 2g -p 5000:80 aspnet48tests
```

### DOCKER RUN FAIL

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

### WIN10 RUN GOOD

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

### SESSION DELETED BECAUSE OF PAGE CRASH

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