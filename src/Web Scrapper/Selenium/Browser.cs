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
    /// <summary>
    /// A class used to represent the Chrome browser
    /// </summary>
    public static class Browser
    {
        private static ChromeOptions browserOptions;
        private static IWebDriver webInstance;
        private static WebDriverWait waitInstance;
        private const string CHROMEDRIVER_DIRECTORY = @"Selenium\Web drivers";
        private static string downloadsDirectory = "Downloads";

        internal static IWebDriver WebInstance { get { return webInstance; } }
        internal static WebDriverWait WaitInstance { get { return waitInstance; } }

        /// <summary>
        /// Return the element location of an HTML element
        /// </summary>
        /// <param name="locationMethod">The location method (supported types: 'xpath, id, linktext, partiallinktext')</param>
        /// <param name="location">The location of the HTML element</param>
        /// <returns>Th BY location method</returns>
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

        /// <summary>
        /// Returns the clickable element given its location method and its location text.
        /// It will wait for the element to be clickable before returning the element
        /// </summary>
        /// <param name="locationMethod">The location method (supported types: 'xpath, id, linktext, partiallinktext')</param>
        /// <param name="location">The location</param>
        /// <returns>The web element</returns>
        internal static IWebElement FindClickableElement(string locationMethod, string location)
        {
            By locator = GetElementLocation(locationMethod, location);

            // Scroll to the viewport of the element
            IWebElement element = webInstance.FindElement(locator);
            IJavaScriptExecutor js = (IJavaScriptExecutor) webInstance;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);

            return waitInstance.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        /// <summary>
        /// Returns the element given its location and the location method type
        /// </summary>
        /// <param name="locationMethod">The location method (supported types: 'xpath, id, linktext, partiallinktext')</param>
        /// <param name="location">The location</param>
        /// <returns>The web element</returns>
        internal static IWebElement FindElement(string locationMethod, string location)
        {
            By locator = GetElementLocation(locationMethod, location);
            IWebElement element = waitInstance.Until(ExpectedConditions.ElementExists(locator));
            return element;
        }

        /// <summary>
        /// Returns a collection of elements given its location and the location method type
        /// </summary>
        /// <param name="locationMethod">The location method (supported types: 'xpath, id, linktext, partiallinktext')</param>
        /// <param name="location">The location of the elements</param>
        /// <returns>A collection of elements</returns>
        internal static IReadOnlyCollection<IWebElement> FindElements(string locationMethod, string location)
        {
            By locator = GetElementLocation(locationMethod, location);
            return webInstance.FindElements(locator);
        }

        /// <summary>
        /// Returns true if the element exists; else false
        /// It will wait for 1 second if the element does not exist before making a final judgemenet
        /// </summary>
        /// <param name="locationMethod">The location method (supported types: 'xpath, id, linktext, partiallinktext')</param>
        /// <param name="location">The location of the element</param>
        /// <returns>True if the element exists; else false</returns>
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

        /// <summary>
        /// Initialize the web browser
        /// </summary>
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
            webInstance = new ChromeDriver(CHROMEDRIVER_DIRECTORY, browserOptions);

            // Create a new wait browser
            waitInstance = new WebDriverWait(webInstance, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Close the web browser
        /// </summary>
        public static void Close()
        {
            webInstance.Dispose();
        }
    }
}
