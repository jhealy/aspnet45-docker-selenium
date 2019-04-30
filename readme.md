# DOCKER WITH SELENIUM AND ASP.NET 4.5 MVC

What am I trying to do?

This is my personal quest to have a  docker image that

* Runs an ASP.NET MVC 4.5 web app
* Has selenium driver via a c# console exe testing the code

Current status - blows out with the WebDriver Timeout error (see below) when SeleneiumDockerText.exe is run inside the docker container.  Occurs with either FireFox or Chrome tests.

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
* Only local?  maybe its blocking http://localhost:port?
* Go 1/1 with some experts that have offered to look at this.
* POSTED ISSUE - Aspnet docker github - https://github.com/Microsoft/aspnet-docker/issues/181
* POSTED ISSUE - StackOverflow - https://stackoverflow.com/questions/55885154/selenium-chromedriver-test-fails-in-windows-docker-container-with-http-request-t
* POSTED ISSUE -  SeleniumHQ - https://groups.google.com/forum/#!topic/selenium-users/3y29poIqvAU
* POSTED ISSUE - Github issue on selenium - Seleniu noted as "not a selenium issue" - https://github.com/SeleniumHQ/selenium/issues/7150

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

## Chrome WebDriver Timeout Error

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

## Firefox web driver timeout error
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

