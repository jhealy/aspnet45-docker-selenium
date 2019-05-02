# DOCKER WITH SELENIUM AND ASP.NET 4.5 MVC

What am I trying to do?

This is my personal quest to have a  docker image that

* Runs an ASP.NET MVC 4.5 web app
* Has selenium driver via a c# console exe testing the code

Current status - when test is run in regular Windows 10 or Windows 2016 it runs fine.  When the test is run in a Windows docker container it blows out with "***session deleted because of page crash***".

I turned on verbose debugging for selenium and trapped out logfiles.  I have two logfiles.  ["goodrun_log.txt"](logs/goodrun_log.txt) is from a Windows 10 successful test.  ["docker_log.txt"](logs/docker_log.txt) is the log from a failed run inside a container.

About line 473 we can see the docker run fail.  Up to that point the log file is exactly the same as the good run.  Then boom.  So what are we missing that makes the docker container fail at that point?  

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

WebDriver Timeout error resolved -Occurs with either FireFox or Chrome tests inside docker container.  FIX (requires both items below):

* Install websocket's into the docker container.  Excerpt from dockerfile:

```powershell
RUN powershell -Command Add-WindowsFeature Web-WebSockets
```

* Pass a very interesting set of options to the chromedriver.

```C#
option.AddArguments( "--headless","--disable-gpu", "--no-sandbox" );
```

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

## WHAT I HAVE TRIED

* Yes, I am running chrome and IE headless
* There are selenium base windows images out there.  They dont have dockerfiles with them so I dont know what's in them. And the newest is 9 months old.  For security and current-ness I want to have a build file using the current windows mcw container repos.
* Chrome and FF both crash with the same error.  I've tried a ton of driver options, none of which work.  Keep suggesting, I'll start enumerating here which ones I've tried.
** Moved up selenium driver from 3.14.0 selenium to 4.0.0-alpha01 drivers….
* EXPOSE PORT - wont work as port varies between runs
* Timeouts up to 130 seconds in various forms after starting driver
* Searched selenium hq for "localhost timed out"
* Searched seleniumhq for "docker timeout remote webdriver"
* WebDriverWait from  https://stackoverflow.com/questions/31336554/selenium-c-webdriverwait-timeout
* Phantomjs is dead for selenium - https://stackoverflow.com/questions/20711407/selenium-webdriver-phantomjs-c-sharp-always-opens-a-cmd-window
Search seleniumhq for docker - nothing there
* changing ff's "--headless" to "-headless".  fyi both work.
* Running in the asp.net 4.6.1 container. Same error - webdriver 60 second timeout (wally)
* fixed 'nogpu' options per https://github.com/SeleniumHQ/selenium/issues/7150 - chrome starts, still fails, new error below
* "--whitelisted-ips=http://localhost,https://localhost" - https://github.com/SeleniumHQ/selenium/commit/32e764df90abfe64f4b4591243da71c4b9dd00a2

## Chrome WebDriver Timeout Error (for posterity)

<pre>
[Target Url] =[http://localhost]
beginning chrome tests
[chrome options:] =[--headless --window-size=1920,1080 --disable-features=VizDisplayCompositor --disable-gpu]
Starting ChromeDriver 74.0.3729.6 (255758eccf3d244491b8a1317aa76e1ce10d57e9-refs/branch-heads/3729@{#29}) on port 49160
Only local connections are allowed.
Please protect ports used by ChromeDriver and related test frameworks to prevent access by malicious code.
[0430/150246.015:ERROR:network_change_notifier_win.cc(156)] WSALookupServiceBegin failed with: 0
[0430/150246.046:ERROR:audio_device_listener_win.cc(46)] RegisterEndpointNotificationCallback failed: 80070424

DevTools listening on ws://127.0.0.1:49163/devtools/browser/853e1883-9876-4fad-9dcc-6bf74c060baf
[0430/150246.253:ERROR:network_change_notifier_win.cc(156)] WSALookupServiceBegin failed with: 0
!!!!!!!!!!!!!!!!!!!!!!!!!!!
[exception caught] =[OpenQA.Selenium.WebDriverException: The HTTP request to the remote WebDriver server for URL http://localhost:49160/session timed out after 60 seconds. ---> System.Net.WebException: The operation has timed out
   at System.Net.HttpWebRequest.GetResponse()
   at OpenQA.Selenium.Remote.HttpCommandExecutor.MakeHttpRequest(HttpRequestInfo requestInfo)
   --- End of inner exception stack trace ---
   at OpenQA.Selenium.Remote.HttpCommandExecutor.MakeHttpRequest(HttpRequestInfo requestInfo)
   at OpenQA.Selenium.Remote.HttpCommandExecutor.Execute(Command commandToExecute)
   at OpenQA.Selenium.Remote.DriverServiceCommandExecutor.Execute(Command commandToExecute)
   at OpenQA.Selenium.Remote.RemoteWebDriver.Execute(String driverCommandToExecute, Dictionary`2 parameters)
   at OpenQA.Selenium.Remote.RemoteWebDriver.StartSession(ICapabilities desiredCapabilities)
   at OpenQA.Selenium.Remote.RemoteWebDriver..ctor(ICommandExecutor commandExecutor, ICapabilities desiredCapabilities)
   at OpenQA.Selenium.Chrome.ChromeDriver..ctor(ChromeDriverService service, ChromeOptions options, TimeSpan commandTimeout)
   at OpenQA.Selenium.Chrome.ChromeDriver..ctor(ChromeOptions options)
   at SeleniumDockerTest.Program.DoChromeTests() in C:\dev\docker-selenium-aspnet45.git\SeleniumDockerTest\Program.cs:line 53]
!!!!!!!!!!!!!!!!!!!!!!!!!!!
Hit any key to continue
</pre>

## Firefox web driver timeout error (for posterity)
<pre>
PS C:\seleniumtests> .\seleniumdockertest.exe http://localhost
[Target Url] =[http://localhost]
firefox tests commencing
BrowserExecutableLocation=C:\Program Files\Mozilla Firefox\firefox.exe
-headless
1556651472894   mozrunner::runner       INFO    Running command: "C:\\Program Files\\Mozilla Firefox\\firefox.exe" "-marionette" "-headless" "-foreground" "-no-remote" "-profile" "C:\\Users\\ContainerAdministrator\\AppData\\Local\\Temp\\rust_mozprofile.hlVVZVvFg66U"
!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
!!!error:OpenQA.Selenium.WebDriverException: The HTTP request to the remote WebDriver server for URL http://localhost:49158/session timed out after 60 seconds. ---> System.Net.WebException: The request was aborted: The operation has timed out.
   at System.Net.HttpWebRequest.GetResponse()
   at OpenQA.Selenium.Remote.HttpCommandExecutor.MakeHttpRequest(HttpRequestInfo requestInfo)
   --- End of inner exception stack trace ---
   at OpenQA.Selenium.Remote.HttpCommandExecutor.MakeHttpRequest(HttpRequestInfo requestInfo)
   at OpenQA.Selenium.Remote.HttpCommandExecutor.Execute(Command commandToExecute)
   at OpenQA.Selenium.Remote.DriverServiceCommandExecutor.Execute(Command commandToExecute)
   at OpenQA.Selenium.Remote.RemoteWebDriver.Execute(String driverCommandToExecute, Dictionary`2 parameters)
   at OpenQA.Selenium.Remote.RemoteWebDriver.StartSession(ICapabilities desiredCapabilities)
   at OpenQA.Selenium.Remote.RemoteWebDriver..ctor(ICommandExecutor commandExecutor, ICapabilities desiredCapabilities)
   at OpenQA.Selenium.Firefox.FirefoxDriver..ctor(FirefoxOptions options)
   at SeleniumDockerTest.Program.DoFirefoxTests() in C:\dev\docker-selenium-aspnet45.git\SeleniumDockerTest\Program.cs:line 150
!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
firefox tests completed
Hit any key to continue
</pre>