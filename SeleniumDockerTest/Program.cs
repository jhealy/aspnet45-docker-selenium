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
            //DoFirefoxTests();

            ConsHelper.Info("skipping firefox test for now");

            // TestChromeDriver();
            DoChromeTests();

            ConsHelper.Pause();
        }

        private static void DoChromeTests()
        {
            ConsHelper.Info("beginning chrome tests");

            IWebDriver chromeDriver;
            try
            {
                ChromeOptions option = new ChromeOptions();
                // base
                //option.AddArguments("--headless");
                //option.AddArguments("--no-sandbox");
                //option.AddArguments("--disable-gpu");

                // https://groups.google.com/a/chromium.org/forum/#!msg/headless-dev/qqbZVZ2IwEw/XMKlEMP3EQAJ
                // must add --headless
                // removed --single-process
                option.AddArguments( "--headless","--disable-gpu", "--no-sandbox", "user-data-dir=/tmp/chrome/user-data", "--homedir=/tmp/chrome", "--disk-cache-dir=/tmp/chrome/cache-dir");

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
                svc.LogPath= "chromelog.txt";

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
                ConsHelper.Err("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                ConsHelper.Err("!!!error:" + ex.ToString());
                ConsHelper.Err("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }
            ConsHelper.Info("firefox tests completed");
        }

    }
}
