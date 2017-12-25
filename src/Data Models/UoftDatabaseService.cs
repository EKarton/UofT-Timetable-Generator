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
    public class UoftDatabaseService
    {
        private static UoftDatabaseService service = null;

        private UoftDatabaseService()
        {
        }

        public static UoftDatabaseService getService()
        {
            if (service == null)
                service = new UoftDatabaseService();
            return service;
        }

        /// <summary>
        /// Get building info from a building code
        /// </summary>
        /// <param name="buildingCode">The building code</param>
        /// <returns>Building information</returns>
        public Building GetBuilding(string buildingCode)
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

        public BuildingDistance GetDistancesBetweenBuildings(Building building1, Building building2)
        {
            using (UofTDataContext db = new UofTDataContext())
            {
                var distances = (from b in db.BuildingDistances
                                 where b.Building.BuildingCode == building1.BuildingCode && b.Building1.BuildingCode == building2.BuildingCode
                                 select b).ToList();

                if (distances.Count == 1)
                {
                    return new BuildingDistance()
                    {
                        Building1 = building1,
                        Building2 = building2,
                        WalkDuration = distances[0].WalkingDuration,
                        WalkingDistance = distances[0].WalkingDistance
                    };
                }
                return null;
            }
        }

        /// <summary>
        /// Get course information given a complete / incomplete UofT course code
        /// Note that the Activities property will be set to null
        /// </summary>
        /// <param name="query">A complete / incomplete UofT course code</param>
        /// <param name="type">How the data is being retrieved</param>
        /// <returns>Course information</returns>
        public List<Course> GetCourses(string courseCodeQuery)
        {
            courseCodeQuery = courseCodeQuery.ToLower();
            List<Course> courses = new List<Course>();

            if (courseCodeQuery.Length == 3)
            {
                using (UofTDataContext db = new UofTDataContext())
                {
                    List<DataContext.usp_GetCourseInfoResult> foundCourses = db.usp_GetCourseInfo(courseCodeQuery).ToList();
                    foreach (DataContext.usp_GetCourseInfoResult course in foundCourses)
                    {

                        courses.Add(new Course()
                        {
                            CourseCode = course.Code,
                            Title = course.Title,
                            Term = course.Term.ToString(),
                            Description = course.Description,
                            Campus = course.Campus
                        });
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
        public Course GetCourseDetails(string courseCode)
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
        public List<Course> GetCourseDetails(string[] courseCodes)
        {
            List<Course> courses = new List<Course>();
            for (int i = 0; i < courseCodes.Length; i++)
                courses.Add(GetCourseDetails(courseCodes[i]));
            return courses;
        }
    }
}
