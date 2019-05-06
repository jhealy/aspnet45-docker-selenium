using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace SeleniumDockerTest
{
    // next up headless
    class Program
    {
        static string m_targetUrl;
        static string sep = @"--------------------------------------------------------------------------------------";

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ConsHelper.Err(@"Please pass a url for the target web as a parameter.   Example:  seleniumdockertext.exe http://localhost:32325");
                return;
            }

            m_targetUrl = args[0].ToString();

            ConsHelper.Msg("Target Url",m_targetUrl);


            // TestFireFoxDriver();
            // DoFirefoxTests();

            ConsHelper.Info("skipping firefox test for now");

            // TestChromeDriver();
            DoChromeTests();

            // same result w page crash
            //DoChromeTests(5);

            ConsHelper.Pause();
        }

        private static void DoChromeTests()
        {
            ConsHelper.Info(sep);
            ConsHelper.Info("SINGLE PASS CHROME TEST, AKA NORMAL");

            IWebDriver chromeDriver;
            try
            {
                ChromeOptions option = new ChromeOptions();

                // https://groups.google.com/a/chromium.org/forum/#!msg/headless-dev/qqbZVZ2IwEw/XMKlEMP3EQAJ
                // must add --headless
                // removed --single-process
                // base arguments to make it work headless
                option.AddArguments( "--headless","--disable-gpu", "--no-sandbox", "--disable-dev-shm-usage");

                // logging options
                option.AddArguments("--user-data-dir=/tmp/chrome/user-data", "--homedir=/tmp/chrome", "--disk-cache-dir=/tmp/chrome/cache-dir");
                option.AddArgument("--log-net-log=/tmp/chrome/netlog.txt");

                // https://github.com/elgalu/docker-selenium/issues/20#issuecomment-407101358
                // option.AddArgument("--disable-dev-shm-usage");

                // NOPE - privileged mode from https://github.com/elgalu/docker-selenium/issues/20
                // option.AddArgument("--privileged");

                // random stuff from https://peter.sh/experiments/chromium-command-line-switches/
                // still crashed in container
                //option.AddArgument("--allow-insecure-localhost");  //enable tls/ssl errors on localhost to be ignored
                //option.AddArgument("--allow-loopback-in-peer-connection"); // allow loopback interface to be added in network list for peer connection
                //option.AddArgument("--allow-no-sandbox-job");
                //option.AddArgument("--also-emit-success-logs");
                //option.AddArgument("--deterministic-fetch"); //Instructs headless_shell to cause network fetches to complete in order of creation.This removes a significant source of network related non - determinism at the cost of slower page loads.
                //option.AddArgument("--enable-crash-reporter"); // Enable crash reporter for headless                                                               
                //option.AddArgument("--enable-tracing"); // enable tracing during browser tests, by default doesnt do much
                //option.AddArgument("--enable-tracing-output=/tmp/chrome/tracingoutput.txt"); // where does tracing go
                //option.AddArgument("--error-console");
                //option.AddArgument("--force-presentation-receiver-for-testing");
                //option.AddArgument("--force-renderer-accessibility");
                //option.AddArgument("--log-level=1");
                //option.AddArgument("--log-net-log=/tmp/chrome/netlog.txt");
                //option.AddArgument("--no-first-run");
                //option.AddArgument("--no-proxy-server");
                //option.AddArgument("--use-fake-device-for-media-stream");
                //option.AddArgument("--use-viz-hit-test"); // WindowServer uses the viz hit-test logic (HitTestAggregator and HitTestQuery)

                // random stuff from https://peter.sh/experiments/chromium-command-line-switches/ that crashes headless
                // option.AddArgument("--dump-dom"); // Instructs headless_shell to print document.body.innerHTML to stdout.
                // option.AddArguments("--trace-startup", "--trace-startup-file=/tmp/chrome/startup.txt");// crashes headless
                // option.AddArguments("--trace-shutdown", "--trace-shutdown-file=/tmp/chrome/shutdown.txt");// crashes headless
                // option.AddArguments("--trace-to-file", "--trace-to-file-name=/tmp/chrome/tracetofile.txt");  // crashes headless

                // option.AddArguments("--disable-dev-shm-usage");
                // fix set from https://bugs.chromium.org/p/chromium/issues/detail?id=942023
                //option.AddArguments("--window-size=1920,1080");
                // option.AddArguments("--disable-features=VizDisplayCompositor");
                // https://github.com/SeleniumHQ/selenium/commit/32e764df90abfe64f4b4591243da71c4b9dd00a2
                // option.AddArguments("--whitelisted-ips=http://localhost,https://localhost");
                //option.AddArgument("--no-sandbox");
                //option.AddArgument("--dns-prefetch-disable");

                ConsHelper.Info("chrome options:", option.DumpArguments());


                // using (chromeDriver = new ChromeDriver(option))
                // driver = new FirefoxDriver(new FirefoxBinary(), profile, new TimeSpan(0, 0, 0, timeoutSeconds));

                ChromeDriverService svc = ChromeDriverService.CreateDefaultService();

                // https://stackoverflow.com/questions/42803545/how-to-enable-chromedriver-logging-in-from-the-selenium-webdriver
                svc.EnableVerboseLogging = true;
                svc.LogPath= @"chromelog.txt";

                using ( chromeDriver = new ChromeDriver(svc, option, TimeSpan.FromSeconds(60)))
                {
                    chromeDriver.Navigate().GoToUrl(m_targetUrl);

                    //TestChromeDriver();
                    string msg = "hello world";
                    Console.WriteLine();
                    ConsHelper.Info($"CheckWebElements('{msg}')", CheckWebElements(msg, chromeDriver).ToString());
                    msg = "Matias";
                    ConsHelper.Info($"CheckWebElements('{msg}')", CheckWebElements(msg, chromeDriver).ToString());

                    chromeDriver.Close();
                    chromeDriver.Quit();
                }
            }
            catch (Exception ex)
            {
                ConsHelper.Err("!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                ConsHelper.Err("exception caught", ex.ToString());
                ConsHelper.Err("!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }
        }

        private static OpenQA.Selenium.Chrome.ChromeDriver GetChromeDriver(ChromeDriverService svc, ChromeOptions options, TimeSpan secstowait )
        {
            return new ChromeDriver(svc, options, secstowait);
        }


        // https://github.com/electron/electron/issues/9369#issuecomment-312234465
        private static void DoChromeTests(int maxNumTrials)
        {
            ConsHelper.Info(sep);
            ConsHelper.Info("beginning chrome tests with WebDriverException failure loop");
            ConsHelper.Info(nameof(maxNumTrials), maxNumTrials.ToString());

            bool isDriverReady = false;
            int trial = 1;

            OpenQA.Selenium.Chrome.ChromeDriver chromeDriver = null;

            try
            {
                ChromeOptions option = new ChromeOptions();
                option.AddArguments("--headless", "--disable-gpu", "--no-sandbox", "--user-data-dir=/tmp/chrome/user-data", "--homedir=/tmp/chrome", "--disk-cache-dir=/tmp/chrome/cache-dir");
                option.AddArgument("--log-net-log=/tmp/chrome/netlog.txt");

                ConsHelper.Info("chrome options:", option.DumpArguments());

                // using (chromeDriver = new ChromeDriver(option))
                // driver = new FirefoxDriver(new FirefoxBinary(), profile, new TimeSpan(0, 0, 0, timeoutSeconds));
                while ( !isDriverReady && (trial <= maxNumTrials ))
                {
                    ConsHelper.Info("chrometest->trial", trial.ToString());
                    try
                    {
                        ChromeDriverService svc = ChromeDriverService.CreateDefaultService();

                        // https://stackoverflow.com/questions/42803545/how-to-enable-chromedriver-logging-in-from-the-selenium-webdriver
                        svc.EnableVerboseLogging = true;
                        svc.LogPath = @"chromelog.txt";

                        chromeDriver = GetChromeDriver(svc, option, TimeSpan.FromSeconds(60));
                        chromeDriver.Navigate().GoToUrl(m_targetUrl);

                        //TestChromeDriver();
                        string msg = "hello world";
                        Console.WriteLine();
                        ConsHelper.Info($"CheckWebElements('{msg}')", CheckWebElements(msg, chromeDriver).ToString());
                        msg = "Matias";
                        ConsHelper.Info($"CheckWebElements('{msg}')", CheckWebElements(msg, chromeDriver).ToString());
                        isDriverReady = true;
                        ConsHelper.Info("completed test trial run", trial.ToString());
                        CloseChromeDriver(chromeDriver);
                    }
                    catch (WebDriverException wde )
                    {
                        trial++;
                        ConsHelper.Err($"!!!! WebDriverException failure on trial# {trial}");
                        ConsHelper.Err(wde.Message);

                        ConsHelper.Info("WebDriverException handler closing ChromeDriver");
                        //CloseChromeDriver(chromeDriver);
                        chromeDriver.Dispose();
                        
                        ConsHelper.Info("sleeping for 1000ms");
                        System.Threading.Thread.Sleep(1000);
                    }
                    catch ( Exception ex )
                    {
                        ConsHelper.Err("!!! general exception caught", ex.ToString());
                        ConsHelper.Info("unrecoverable, exiting test");
                        CloseChromeDriver(chromeDriver);
                        return;
                    }
                    if ( ! isDriverReady )
                    {
                        ConsHelper.Err($"could not keep driver alive after {maxNumTrials} attempts");
                    }
                } // while
            }
            catch (Exception ex)
            {
                ConsHelper.Err("!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                ConsHelper.Err("exception caught", ex.ToString());
                ConsHelper.Err("!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }      
        }

        private static void CloseChromeDriver(ChromeDriver chromeDriver)
        {
            try
            {
                chromeDriver.Close();
                chromeDriver.Quit();
                chromeDriver.Dispose();
            }
            catch (Exception ex)
            {
                ConsHelper.Err("!!! error closing down chromedriver", ex.ToString());
            }
        }

        static public bool CheckWebElements(string msg, IWebDriver driver)
        {
            if (string.IsNullOrWhiteSpace(msg)) return false;
            msg = msg.ToLower();
            return driver.FindElement(By.Id("myH1")).Text.ToLower().Contains(msg);
        }

        static public void TestChromeDriver()
        {
            ChromeOptions option = new ChromeOptions();
            // option.AddArguments("--headless");
            // fix set from https://bugs.chromium.org/p/chromium/issues/detail?id=942023
            option.AddArguments("--window-size=1920,1080");
            //option.AddArguments("--disable-features=VizDisplayCompositor");
            option.AddArguments("--disable-gpu");

            using (IWebDriver chromeDriver = new ChromeDriver(option))
            {
                chromeDriver.Navigate().GoToUrl(m_targetUrl);
                chromeDriver.FindElement(By.Id("myText")).SendKeys("probably nothing");
            }
        }

        static public void TestFireFoxDriver()
        {
            Console.WriteLine("testing firefox driver");
            try
            {
                FirefoxOptions options = new FirefoxOptions();
                string fflocation = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                options.BrowserExecutableLocation = fflocation;
                Console.WriteLine("BrowserExecutableLocation=" + fflocation);
                options.AddArgument("-headless");
                Console.WriteLine("-headless");


                using (IWebDriver ffDriver = new FirefoxDriver(options))
                {

                    ffDriver.Navigate().GoToUrl(m_targetUrl);
                    ffDriver.FindElement(By.Id("myText")).SendKeys("probably nothing");
                }
            }
            catch (Exception ex)
            {
                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"!!!" + ex.ToString());
                Console.ForegroundColor = oldColor;
            }

            Console.WriteLine("firefox driver test run completed");
            // 

        }
            private static void DoFirefoxTests()
        {
            ConsHelper.Info(sep);

            ConsHelper.Info("firefox tests commencing");
            string fflocation = @"C:\Program Files\Mozilla Firefox\firefox.exe";
            ConsHelper.Info("BrowserExecutableLocation=" + fflocation);
            IWebDriver firefoxDriver;
            try
            {
                FirefoxOptions options = new FirefoxOptions();
                options.BrowserExecutableLocation = fflocation;                
                options.AddArgument("-headless");
                ConsHelper.Info("-headless");


                // fix attempt from jetman1981 @ https://www.bountysource.com/issues/54757000-the-http-request-to-the-remote-webdriver-server-for-url-http-localhost-42607-session-timed-out-after-60-seconds
                // same timeout error 
                // Console.WriteLine(@"fix attempt from jetman1981 @ https://www.bountysource.com/issues/54757000-the-http-request-to-the-remote-webdriver-server-for-url-http-localhost-42607-session-timed-out-after-60-seconds");
                // using ( firefoxDriver = new FirefoxDriver( ".", options, TimeSpan.FromSeconds(130)))

                using (firefoxDriver = new FirefoxDriver(options))
                {
                    firefoxDriver.Navigate().GoToUrl(m_targetUrl);

                    // did not resolve timeout issues
                    // Console.WriteLine("trying hack from https://stackoverflow.com/questions/31336554/selenium-c-webdriverwait-timeout");
                    // OpenQA.Selenium.Support.UI.WebDriverWait wait = new OpenQA.Selenium.Support.UI.WebDriverWait(firefoxDriver, TimeSpan.FromSeconds(40));
                    // wait.Until(d => d.FindElement(By.Id("myH1")));

                    //TestChromeDriver();
                    string msg = "hello world";
                    ConsHelper.Info($"CheckWebElements('{msg}')={CheckWebElements(msg, firefoxDriver)}");
                    msg = "Matias";
                    ConsHelper.Info($"CheckWebElements('{msg}')={CheckWebElements(msg, firefoxDriver)}");

                    firefoxDriver.Close();
                    firefoxDriver.Quit();
                }
            }
            catch (Exception ex)
            {
                ConsHelper.Err(sep);
                ConsHelper.Err("!!!error:" + ex.ToString());
                ConsHelper.Err(sep);
            }
            ConsHelper.Info("firefox tests completed");
        }

    }

}
