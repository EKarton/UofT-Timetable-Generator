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
    internal class CourseListScrapper
    {
        public void GetCourseList()
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

            CourseList courseNames = new CourseList() { CourseNames = courses };
            string json = JsonConvert.SerializeObject(courseNames);
            File.WriteAllText("Course List.json", json);

            Browser.Close();
        }
    }
}
