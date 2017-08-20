using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataContext;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.DataModels
{
    public static class UoftDatabaseService
    {
        public enum CourseQueryType
        {
            CourseCode
        }

        public static Building GetBuilding(string buildingCode)
        {
            using (UoftDataContext db = new UoftDataContext())
            {
                var building = (from b in db.Buildings
                                where b.BuildingCode == buildingCode
                                select b).ToList();

                if (building.Count != 1)
                    throw new Exception("There is more than one or no buildings with building code " + buildingCode);

                return new Building(building[0]);
            }
        }

        public static List<Course> GetCourses(string query, CourseQueryType type)
        {
            query = query.ToLower();
            List<Course> courses = new List<Course>();

            if (type == CourseQueryType.CourseCode && query.Length >= 3)
            {
                using (UoftDataContext db = new UoftDataContext())
                {
                    List<DataContext.Course> dbCourses = db.Courses.ToList();
                    foreach (DataContext.Course c in dbCourses)
                        if (c.Code.ToLower().Contains(query))
                        {
                            Course newCourse = new Course()
                            {
                                CourseCode = c.Code,
                                Activities = null,
                                Term = c.Term.ToString(),
                                Title = ""
                            };
                            courses.Add(newCourse);
                        }
                }
            }
            return courses;
        }

        public static Course GetCourseDetails(string courseCode)
        {
            using (UoftDataContext db = new UoftDataContext())
            {
                List<DataContext.Course> oldCourses = (from c in db.Courses
                                                       where c.Code == courseCode
                                                       select c).ToList();

                if (oldCourses.Count() != 1)
                    throw new Exception("There is more than one or no course with course code " + courseCode);

                DataContext.Course oldCourse = oldCourses[0];
                return new Course(oldCourse);
            }
        }

        public static List<Course> GetCourseDetails(string[] courseCodes)
        {
            List<Course> courses = new List<Course>();
            for (int i = 0; i < courseCodes.Length; i++)
                courses.Add(GetCourseDetails(courseCodes[i]));
            return courses;
        }
    }
}
