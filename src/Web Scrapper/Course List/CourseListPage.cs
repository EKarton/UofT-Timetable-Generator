using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.WebScrapper
{
    public static class CourseListPage
    {
        public static int CurrentPage
        {
            get
            {
                string pageText = Browser.FindElement("xpath", "//*[@id=\"block-system-main\"]/div/div/div[3]/ul/li[2]").Text;
                string curPage = pageText.Replace(" of ", " ").Split(' ')[0];
                return Convert.ToInt32(curPage);
            }
        }

        public static int MaxPages
        {
            get
            {
                string pageText = Browser.FindElement("xpath", "//*[@id=\"block-system-main\"]/div/div/div[3]/ul/li[2]").Text;
                string lastPage = pageText.Replace(" of ", " ").Split(' ')[1];
                return Convert.ToInt32(lastPage);
            }
        }

        public static bool IsAtLastPage()
        {
            return CurrentPage == MaxPages;
        }

        public static void GotoNextPage()
        {
            Browser.FindElement("xpath", "//*[@id=\"block-system-main\"]/div/div/div[3]/ul/li[3]/a").Click();
        }

        public static void GotoPreviousPage()
        {
            Browser.FindElement("xpath", "//*[@id=\"block-system-main\"]/div/div/div[3]/ul/li[1]/a").Click();
        }

        public static List<string> GetCoursesOnPage()
        {
            List<string> courses = new List<string>();
            IReadOnlyCollection<IWebElement> rows = Browser.FindElements("xpath", "//*[@id=\"block-system-main\"]/div/div/div[2]/table/tbody/tr");
            foreach (IWebElement row in rows)
            {
                string courseID = row.FindElement(By.XPath("td[1]")).Text;
                courses.Add(courseID);
            }

            return courses;
        }
    }
}
