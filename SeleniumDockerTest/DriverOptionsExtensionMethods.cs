using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;

namespace SeleniumDockerTest
{
    public static class SeleneiumExtensionMethods
    {
        public static string DumpArguments( this OpenQA.Selenium.Chrome.ChromeOptions options )
        {
            if (options == null || options.Arguments.Count <= 0) return string.Empty;
            StringBuilder sb = new StringBuilder(1024);
            foreach( string arg in options.Arguments )
            {
                sb.Append(arg + " ");
            }
            return sb.ToString().Trim();
        }

        // firefox doesn't expose arguments publicly
        // public static string DumpArguments(this OpenQA.Selenium.Firefox.FirefoxOptions options)
    }
}
