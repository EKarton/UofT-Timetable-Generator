using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.WebScrapper
{
    internal class CourseInfoScrapper : IWebScrapper
    {
        public void Run()
        {
            Browser.Initialize();
            Browser.WebInstance.Url = "https://timetable.iit.artsci.utoronto.ca/";

            // Grab course list from a file
            CourseList courseList = JsonConvert.DeserializeObject<CourseList>(File.ReadAllText(FileLocations.COURSELIST_FILENAME));

            foreach (string courseID in courseList.Courses)
            {
                List<Course> coursesInfo = TimetablePage.GetCourseInfo(courseID);
                foreach (var course in coursesInfo)
                {
                    string filePath = FileLocations.COURSEINFO_DIRECTORY + "\\" + course.CourseName + ".json";
                    File.WriteAllText(filePath, JsonConvert.SerializeObject(course, Formatting.Indented));
                }
            }
            Browser.Close();
        }
    }
}
