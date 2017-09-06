using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace UoftTimetableGenerator.WebScrapper
{
    /// <summary>
    /// Get the courses present in the UofT Course Calendar page
    /// </summary>
    internal class CourseListScrapper
    {
        /// <summary>
        /// Get the courses present in the current page of the UofT course calendar
        /// </summary>
        /// <returns>The courses present</returns>
        public List<Course> GetCoursesOnPage()
        {
            var parsedCourses = new List<Course>();
            var rows = Browser.FindElements("xpath", "//*[@id='block-system-main']/div/div/div[2]/table/tbody/tr");
            foreach (IWebElement row in rows)
            {
                var cells = row.FindElements(By.TagName("td"));
                string courseCode = cells[0].Text.Trim();
                string courseTitle = cells[1].Text.Trim();
                string courseDescription = cells[2].Text.Trim();

                var newCourse = new Course()
                {
                    Code = courseCode,
                    Title = courseTitle,
                    Description = courseDescription,
                    Campus = "St. George"
                };
                parsedCourses.Add(newCourse);
            }
            return parsedCourses;
        }

        /// <summary>
        /// Get all the courses in all of the pages present in the UofT Course Calendar, 
        /// and save it in a .json format called "courselist.json"
        /// </summary>
        public void GetCourseList()
        {
            // Start and navigate the browser to the webpage containing all the courses
            Browser.Initialize();
            Browser.WebInstance.Url = "https://fas.calendar.utoronto.ca/search-courses";

            // Get a list of courses and save it
            List<Course> courses = new List<Course>();
            bool isAtLastPage = false;
            do
            {
                var coursesOnPage = GetCoursesOnPage();
                courses.AddRange(coursesOnPage);

                if (CourseListPage.IsAtLastPage())
                    isAtLastPage = true;
                else
                    CourseListPage.GotoNextPage();
            }
            while (!isAtLastPage);

            string json = JsonConvert.SerializeObject(courses);
            File.WriteAllText("courses.json", json);

            Browser.Close();
        }
    }
}
