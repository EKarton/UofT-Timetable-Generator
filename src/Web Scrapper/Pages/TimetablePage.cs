using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.WebScrapper
{
    /// <summary>
    /// A class used to model the behaviours and actions in the UofT Timetables page
    /// </summary>
    internal static class TimetablePage
    {
        /// <summary>
        /// Navigates the browser to the UofT Timetables page
        /// </summary>
        public static void GotoPage()
        {
            Browser.WebInstance.Url = @"https://timetable.iit.artsci.utoronto.ca/";
        }

        /// <summary>
        /// Waits for the course content to load
        /// </summary>
        public static void WaitForContentToLoad()
        {
            // If the element does not exist, skip it.
            if (Browser.DoesElementExist("id", "jsspinner") == false)
                return;

            // Make the timeout longer
            TimeSpan prevTimeout = Browser.WaitInstance.Timeout;
            TimeSpan prevPollInterval = Browser.WaitInstance.PollingInterval;
            Browser.WaitInstance.Timeout = TimeSpan.FromSeconds(300);
            Browser.WaitInstance.PollingInterval = TimeSpan.FromMilliseconds(100);

            // Wait until the page finished loading
            try
            {
                Browser.WaitInstance.Until(delegate (IWebDriver driver)
                {
                    return !driver.FindElement(By.Id("jsspinner")).Displayed;
                });

                // Reset the timeouts back to default
                Browser.WaitInstance.Timeout = prevTimeout;
                Browser.WaitInstance.PollingInterval = prevPollInterval;
            }
            catch { throw new Exception("Content took too long to load!"); }
        }

        /// <summary>
        /// Clicks on the search button
        /// </summary>
        public static void SearchForCourses()
        {
            Browser.FindClickableElement("xpath", "//*[@id='searchButton']").Click();
        }

        /// <summary>
        /// Enters the couse code to the course code textbox
        /// </summary>
        /// <param name="courseID">The course code</param>
        public static void EnterCourseCode(string courseID)
        {
            Browser.FindElement("id", "courseCode").SendKeys(courseID);
        }

        /// <summary>
        /// Selects the terms in the terms textbox
        /// </summary>
        /// <param name="terms">A list of terms</param>
        public static void SelectTerm(string[] terms)
        {
            Browser.FindClickableElement("xpath", "//*[@id='js-modal-page']/div/div[2]/div[1]/div[3]/div[1]/div[3]/div[2]/div[1]").Click();

            foreach (string term in terms)
            {
                var dropdownOptions = Browser.FindElements("xpath", "//*[@id='js-modal-page']/div/div[2]/div[1]/div[3]/div[1]/div[3]/div[2]/div[2]/div/div");
                foreach (IWebElement option in dropdownOptions)
                {
                    if (option.Text == term)
                    {
                        option.Click();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Clicks on the Clear Filters button
        /// </summary>
        public static void ClearFilters()
        {
            Browser.FindClickableElement("id", "btnClear").Click();
        }
    }
}
