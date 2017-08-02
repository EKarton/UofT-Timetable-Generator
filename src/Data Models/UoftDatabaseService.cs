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

        public static Course GetCourse(string courseCode)
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

        public static List<Course> GetCourses(string[] courseCodes)
        {
            List<Course> courses = new List<Course>();
            for (int i = 0; i < courseCodes.Length; i++)
                courses.Add(GetCourse(courseCodes[i]));
            return courses;
        }
    }
}
