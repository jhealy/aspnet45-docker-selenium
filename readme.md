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

* Go 1/1 with some experts that have offered to look at this.
* Watch StackOverflow - https://stackoverflow.com/questions/55885154/selenium-chromedriver-test-fails-in-windows-docker-container-with-http-request-t
* Watch SeleniumHQ - pending url
* Watch github issue on selenium - https://github.com/SeleniumHQ/selenium/issues/7150

## WHAT I HAVE TRIED

* Yes, I am running chrome and IE headless
* There are selenium base windows images out there.  They dont have dockerfiles with them so I dont know what's in them. And the newest is 9 months old.  For security and current-ness I want to have a build file using the current windows mcw container repos.
* Chrome and FF both crash with the same error.  I've tried a ton of driver options, none of which work.  Keep suggesting, I'll start enumerating here which ones I've tried.
** Moved up selenium driver from 3.14.0 selenium to 4.0.0-alpha01 drivers….
* EXPOSE PORT - wont work as port varies between runs
* Timeouts up to 130 seconds in various forms after starting driver
* Searched selenium hq for "localhost timed out"
* Searched seleniumhq for "docker timeout remote webdriver"
* *WebDriverWait from  https://stackoverflow.com/questions/31336554/selenium-c-webdriverwait-timeout
* Phantomjs is dead for selenium - https://stackoverflow.com/questions/20711407/selenium-webdriver-phantomjs-c-sharp-always-opens-a-cmd-window
Search seleniumhq for docker - nothing there

## WebDriver Timeout Error

<pre>
PS C:\seleniumtests> .\SeleniumDockerTest.exe
Please pass a url for the target web as a parameter.   Example:  seleniumdockertext.exe http://localhost:32325
PS C:\seleniumtests> .\SeleniumDockerTest.exe http://localhost
Target Url=http://localhost
chrome options include --headless --dns-prefetch-disable --disablefeatuers
Starting ChromeDriver 74.0.3729.6 (255758eccf3d244491b8a1317aa76e1ce10d57e9-refs/branch-heads/3729@{#29}) on port 49164
Only local connections are allowed.
Please protect ports used by ChromeDriver and related test frameworks to prevent access by malicious code.
[0427/172326.658:ERROR:network_change_notifier_win.cc(156)] WSALookupServiceBegin failed with: 0
[0427/172326.673:ERROR:audio_device_listener_win.cc(46)] RegisterEndpointNotificationCallback failed: 80070424

DevTools listening on ws://127.0.0.1:49167/devtools/browser/920800fc-253a-4443-881d-925c21230f96
[0427/172326.783:ERROR:browser_gpu_channel_host_factory.cc(139)] Failed to launch GPU process.
[0427/172326.910:ERROR:network_change_notifier_win.cc(156)] WSALookupServiceBegin failed with: 0
!!!error:OpenQA.Selenium.WebDriverException: The HTTP request to the remote WebDriver server for URL http://localhost:49164/session timed out after 60 seconds. ---> System.Net.WebException: The operation has timed out
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
   at SeleniumDockerTest.Program.Main(String[] args) in C:\dev\devfish-shares.git\docker-selenium-aspnet45\SeleniumDockerTest\Program.cs:line 44
</pre>


