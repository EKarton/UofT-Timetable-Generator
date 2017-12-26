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
    /// A class used to retrieve university data.
    /// It uses the Singleton design pattern where only one instance of UoftServices can be made.
    /// </summary>
    public class UoftServices
    {
        private static UoftServices service = null;
        private Dictionary<Tuple<string, string>, BuildingDistance> distanceCache = new Dictionary<Tuple<string, string>, BuildingDistance>();
        private const int MAX_CACHESIZE = 100;

        /// <summary>
        /// A constructor made private so that code outside of this class cannot instantiate this class.
        /// </summary>
        private UoftServices()
        {
        }

        /// <summary>
        /// Returns the current Uoft service
        /// </summary>
        /// <returns></returns>
        public static UoftServices GetService()
        {
            if (service == null)
                service = new UoftServices();
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

        /// <summary>
        /// Return the time and distance between two buildings.
        /// </summary>
        /// <param name="building1"></param>
        /// <param name="building2"></param>
        /// <returns></returns>
        public BuildingDistance GetDistancesBetweenBuildings(Building building1, Building building2)
        {
            Tuple<string, string> buildingsInfo = new Tuple<string, string>(building1.Address, building2.Address);

            if (distanceCache.Count > MAX_CACHESIZE)
                distanceCache.Clear();

            if (distanceCache.ContainsKey(buildingsInfo))
            {
                return distanceCache[buildingsInfo];
            }
            else
            {
                BuildingDistanceCalculator finder = new BuildingDistanceCalculator();
                BuildingDistance distance = finder.GetInfoBetweenBuildings("walking", building1.Address, building2.Address);
                distanceCache.Add(buildingsInfo, distance);
                return distance;
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
