using DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.DataModels
{
    /// <summary>
    /// A class used to retrieve data from the database
    /// </summary>
    public static class UoftDatabaseService
    {
        /// <summary>
        /// The ways to retrieve course data from the database
        /// </summary>
        public enum CourseQueryType
        {
            CourseCode
        }

        /// <summary>
        /// Get building info from a building code
        /// </summary>
        /// <param name="buildingCode">The building code</param>
        /// <returns>Building information</returns>
        public static Building GetBuilding(string buildingCode)
        {
            using (UofTDataContext db = new UofTDataContext())
            {
                var building = (from b in db.Buildings
                                where b.BuildingCode == buildingCode
                                select b).ToList();

                if (building.Count != 1)
                    throw new Exception("There is more than one or no buildings with building code " + buildingCode);

                return new Building(building[0]);
            }
        }

        /// <summary>
        /// Get course information given a complete / incomplete UofT course code
        /// Note that the Activities property will be set to null
        /// </summary>
        /// <param name="query">A complete / incomplete UofT course code</param>
        /// <param name="type">How the data is being retrieved</param>
        /// <returns>Course information</returns>
        public static List<Course> GetCourses(string query, CourseQueryType type)
        {
            query = query.ToLower();
            List<Course> courses = new List<Course>();

            if (type == CourseQueryType.CourseCode && query.Length >= 3)
            {
                using (UofTDataContext db = new UofTDataContext())
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

        /// <summary>
        /// Get complete course information (including the activities, sections, sessions, etc)
        /// of a course
        /// </summary>
        /// <param name="courseCode">A complete UofT course code</param>
        /// <returns>Course information</returns>
        public static Course GetCourseDetails(string courseCode)
        {
            using (UofTDataContext db = new UofTDataContext())
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

        /// <summary>
        /// Get complete course details (including the activities, sections, and sessions for each course) 
        /// on a list of course codes
        /// </summary>
        /// <param name="courseCodes">A list of complete UofT course codes</param>
        /// <returns>Complete course information for each course</returns>
        public static List<Course> GetCourseDetails(string[] courseCodes)
        {
            List<Course> courses = new List<Course>();
            for (int i = 0; i < courseCodes.Length; i++)
                courses.Add(GetCourseDetails(courseCodes[i]));
            return courses;
        }
    }
}
