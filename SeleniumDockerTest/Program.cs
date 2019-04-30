using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

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
                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"Please pass a url for the target web as a parameter.   Example:  seleniumdockertext.exe http://localhost:32325");
                Console.ForegroundColor = oldColor;

                // Console.WriteLine("tests complete, hit enter to exit");
                // Console.ReadLine();

                return;
            }

            m_targetUrl = args[0].ToString();

            Console.WriteLine($"Target Url={m_targetUrl}");

            // TestChromeDriver();
            //DoChromeTests();

            // TestFireFoxDriver();
            DoFirefoxTests();

            Console.WriteLine("hit any key to continue");
            Console.ReadLine();
        }

        private static void DoChromeTests()
        {
            IWebDriver chromeDriver;
            try
            {
                ChromeOptions option = new ChromeOptions();
                option.AddArguments("--headless");
                // fix set from https://bugs.chromium.org/p/chromium/issues/detail?id=942023
                option.AddArguments("--window-size=1920,1080");
                option.AddArguments("--disable-features=VizDisplayCompositor");
                option.AddArguments("--disable-gpu");

                //option.AddArgument("--no-sandbox");
                //option.AddArgument("--dns-prefetch-disable");


                Console.WriteLine("chrome options include --headless --dns-prefetch-disable --disablefeatures");
                using (chromeDriver = new ChromeDriver(option))
                {
                    chromeDriver.Navigate().GoToUrl(m_targetUrl);

                    //TestChromeDriver();
                    string msg = "hello world";
                    Console.WriteLine($"CheckWebElements('{msg}')={CheckWebElements(msg,chromeDriver)}");
                    msg = "Matias Bruno";
                    Console.WriteLine($"CheckWebElements('{msg}')={CheckWebElements(msg,chromeDriver)}");

                    //Console.WriteLine("tests complete, hit enter to exit");
                    //Console.ReadLine();

                    chromeDriver.Close();
                    chromeDriver.Quit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("!!!error:" + ex.ToString());
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

        //static public void TestFireFoxDriver()
        //{
        //    Console.WriteLine("testing firefox driver");
        //    try
        //    {
        //        FirefoxOptions options = new FirefoxOptions();
        //        string fflocation = @"C:\Program Files\Mozilla Firefox\firefox.exe";
        //        options.BrowserExecutableLocation = fflocation;
        //        Console.WriteLine("BrowserExecutableLocation=" + fflocation);
        //        options.AddArgument("-headless");
        //        Console.WriteLine("-headless");


        //        using (IWebDriver ffDriver = new FirefoxDriver(options))
        //        {

        //            ffDriver.Navigate().GoToUrl(m_targetUrl);
        //            ffDriver.FindElement(By.Id("myText")).SendKeys("probably nothing");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ConsoleColor oldColor = Console.ForegroundColor;
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.WriteLine(@"!!!" + ex.ToString());
        //        Console.ForegroundColor = oldColor;
        //    }

        //    Console.WriteLine("firefox driver test run completed");
        // }

        private static void DoFirefoxTests()
        {
            Console.WriteLine("firefox tests commencing");
            string fflocation = @"C:\Program Files\Mozilla Firefox\firefox.exe";
            Console.WriteLine("BrowserExecutableLocation=" + fflocation);
            IWebDriver firefoxDriver;
            try
            {
                FirefoxOptions options = new FirefoxOptions();
                options.BrowserExecutableLocation = fflocation;                
                options.AddArgument("-headless");
                Console.WriteLine("-headless");


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
                    Console.WriteLine($"CheckWebElements('{msg}')={CheckWebElements(msg, firefoxDriver)}");
                    msg = "Matias";
                    Console.WriteLine($"CheckWebElements('{msg}')={CheckWebElements(msg, firefoxDriver)}");

                    firefoxDriver.Close();
                    firefoxDriver.Quit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("!!!error:" + ex.ToString());
            }
            Console.WriteLine("firefox tests completed");
        }

    }
}
