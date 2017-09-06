using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.WebScrapper
{
    /// <summary>
    /// A class that models the UofT course calendar page
    /// </summary>
    public static class CourseListPage
    {
        /// <summary>
        /// Get the current page number
        /// </summary>
        public static int CurrentPage
        {
            get
            {
                string pageText = Browser.FindElement("xpath", "//*[@id=\"block-system-main\"]/div/div/div[3]/ul/li[2]").Text;
                string curPage = pageText.Replace(" of ", " ").Split(' ')[0];
                return Convert.ToInt32(curPage);
            }
        }

        /// <summary>
        /// Get the number of pages
        /// </summary>
        public static int MaxPages
        {
            get
            {
                string pageText = Browser.FindElement("xpath", "//*[@id=\"block-system-main\"]/div/div/div[3]/ul/li[2]").Text;
                string lastPage = pageText.Replace(" of ", " ").Split(' ')[1];
                return Convert.ToInt32(lastPage);
            }
        }

        /// <summary>
        /// Determines whether it is at the last page
        /// </summary>
        /// <returns>Returns true if it is at the last page; else false</returns>
        public static bool IsAtLastPage()
        {
            return CurrentPage == MaxPages;
        }

        /// <summary>
        /// Navigates to the next page
        /// </summary>
        public static void GotoNextPage()
        {
            Browser.FindElement("xpath", "//*[@id=\"block-system-main\"]/div/div/div[3]/ul/li[3]/a").Click();
        }

        /// <summary>
        /// Navigates to the previous page
        /// </summary>
        public static void GotoPreviousPage()
        {
            Browser.FindElement("xpath", "//*[@id=\"block-system-main\"]/div/div/div[3]/ul/li[1]/a").Click();
        }
    }
}
