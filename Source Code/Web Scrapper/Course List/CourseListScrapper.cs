using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.WebScrapper
{
    internal class CourseListScrapper : IWebScrapper
    {
        public void Run()
        {
            // Start and navigate the browser to the webpage containing all the courses
            Browser.Initialize();
            Browser.WebInstance.Url = "https://fas.calendar.utoronto.ca/search-courses";

            // Get a sorted list of courses
            List<string> courses = new List<string>();
            bool isAtLastPage = false;
            do
            {
                var coursesOnPage = CourseListPage.GetCoursesOnPage();
                courses.AddRange(coursesOnPage);

                if (CourseListPage.IsAtLastPage())
                    isAtLastPage = true;
                else
                    CourseListPage.GotoNextPage();
            }
            while (!isAtLastPage);
            courses.Sort();

            // Save it in a .json file and close the browser.
            CourseList courseList = new CourseList() { Courses = courses.ToArray() };
            File.WriteAllText(FileLocations.COURSELIST_FILENAME, JsonConvert.SerializeObject(courseList));
            Browser.Close();
        }
    }
}
