using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;

namespace UoftTimetableGenerator.WebScrapper
{
    public static class Browser
    {
        private static ChromeOptions browserOptions;
        private static IWebDriver webInstance;
        private static WebDriverWait waitInstance;
        private static string downloadsDirectory = "Downloads";

        internal static IWebDriver WebInstance { get { return webInstance; } }
        internal static WebDriverWait WaitInstance { get { return waitInstance; } }

        private static By GetElementLocation(string locationMethod, string location)
        {
            switch (locationMethod)
            {
                case "xpath":
                    return By.XPath(location);
                case "id":
                    return By.Id(location);
                case "linktext":
                    return By.LinkText(location);
                case "partiallinktext":
                    return By.PartialLinkText(location);

                default:
                    throw new NotImplementedException("This type of location method was not handled before!");
            }
        }

        internal static IWebElement FindClickableElement(string locationMethod, string location)
        {
            By locator = GetElementLocation(locationMethod, location);
            return waitInstance.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        internal static IWebElement FindElement(string locationMethod, string location)
        {
            By locator = GetElementLocation(locationMethod, location);
            IWebElement element = waitInstance.Until(ExpectedConditions.ElementExists(locator));
            return element;
        }

        internal static IReadOnlyCollection<IWebElement> FindElements(string locationMethod, string location)
        {
            By locator = GetElementLocation(locationMethod, location);
            return webInstance.FindElements(locator);
        }

        internal static bool DoesElementExist(string locationMethod, string location)
        {
            // Make the duration of checking shorter
            TimeSpan prevTimeout = waitInstance.Timeout;
            waitInstance.Timeout = TimeSpan.FromSeconds(1);

            // Check
            bool doesElementExist = false;
            try
            {
                By locator = GetElementLocation(locationMethod, location);
                waitInstance.Until(ExpectedConditions.ElementExists(locator));
                doesElementExist = true;
            }
            catch { }

            // Reset the duration
            waitInstance.Timeout = prevTimeout;
            return doesElementExist;
        }

        public static void Initialize()
        {
            // Set the browser options so that any file to be downloaded downloads to the Downloads folder
            browserOptions = new ChromeOptions();
            browserOptions.AddUserProfilePreference("download.default_directory", downloadsDirectory);
            browserOptions.AddUserProfilePreference("download.prompt_for_download", false);
            browserOptions.AddUserProfilePreference("download.directory_upgrade", true);
            browserOptions.AddUserProfilePreference("safebrowsing.enabled", true);
            browserOptions.AddUserProfilePreference("plugins.always_open_pdf_externally", true);

            // Create the new browser
            webInstance = new ChromeDriver(browserOptions);

            // Create a new wait browser
            waitInstance = new WebDriverWait(webInstance, TimeSpan.FromSeconds(10));
        }

        public static void Close()
        {
            webInstance.Dispose();
        }
    }
}
