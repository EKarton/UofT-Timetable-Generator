using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UoftTimetableGenerator.DataModels;
using UoftTimetableGenerator.Generator;

namespace UoftTimetableGeneratorTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<Course> courses = new List<Course>();
            //courses = UoftDatabaseService.GetCourseDetails(new string[] { "MAT137Y1-Y", "COG250Y1-Y", "CSC148H1-F", "CSC165H1-F", "ENV100H1-F" });
            courses = UoftDatabaseService.GetCourseDetails(new string[] { "MAT133Y1-Y", "MAT135H1-F" });

            // Default preferences
            Preferences preferences = new Preferences()
            {
                ClassType = Preferences.Day.Undefined,
                WalkingDistance = Preferences.Quantity.Undefined,
                NumDaysInClass = Preferences.Quantity.Undefined,
                TimeBetweenClasses = Preferences.Quantity.Undefined,
                LunchPeriod = 0
            };

            // Default restrictions
            Restrictions restrictions = new Restrictions()
            {
                EarliestClass = null,
                LatestClass = null,
                WalkDurationInBackToBackClasses = 20
            };

            GAGenerator<YearlyTimetable> generator = new GAGenerator<YearlyTimetable>(courses, preferences, restrictions);

            var timetables = generator.GetTimetables();
            foreach (var timetable in timetables)
            {
                Console.WriteLine("\n\n#####################################################################");
                timetable.Show();
            }
        }
    }
}
