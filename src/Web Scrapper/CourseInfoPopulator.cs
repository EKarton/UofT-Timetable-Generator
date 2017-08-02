using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataContext;

namespace UoftTimetableGenerator.WebScrapper
{
    public class CourseInfoPopulator
    {
        public void InsertCourseInfoToDatabase(bool redoEntireTable)
        {
            using (UoftDataContext db = new UoftDataContext())
            {
                CourseList courseList = JsonConvert.DeserializeObject<CourseList>(File.ReadAllText("Course List.json"));
                foreach (string courseCode in courseList.CourseNames)
                {
                    var existingCourses = from c in db.Courses
                                          where c.Code.Contains(courseCode)
                                          select c;
                }
            }
        }
    }
}
