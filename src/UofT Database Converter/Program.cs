using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UofT_Database_Converter
{
    class Program
    {
        static New.NewDBDataContext newDB = new New.NewDBDataContext();
        static Old.OldDBDataContext oldDB = new Old.OldDBDataContext();

        static int courseID = 0;
        static int activityID = 0;
        static int sectionID = 0;
        static int sessionID = 0;

        static void AddSession(Old.Session oldSession, New.Section newSection)
        {
            var building1Results = from p in newDB.Buildings
                                    where p.Address == oldSession.Building.Address
                                    select p.BuildingID;

            var building2Results = from p in newDB.Buildings
                                    where p.Address == oldSession.Building1.Address
                                    select p.BuildingID;

            int? building1ID = null;
            if (oldSession.Fall_BuildingID != null && building1Results != null && building1Results.ToList().Count == 1)
                building1ID = building1Results.ToList()[0];

            int? building2ID = null;
            if (oldSession.Winter_BuildingID != null && building2Results != null && building2Results.ToList().Count == 1)
                building2ID = building2Results.ToList()[0];

            var newSession = new New.Session()
            {
                SectionID = newSection.SectionID,
                StartTime = oldSession.StartTime,
                EndTime = oldSession.EndTime,
                Fall_RoomNumber = oldSession.Fall_RoomNumber,
                Fall_BuildingID = building1ID,
                Winter_RoomNumber = oldSession.Winter_RoomNumber,
                Winter_BuildingID = building2ID
            };
            sessionID++;
            newDB.Sessions.InsertOnSubmit(newSession);
            newDB.SubmitChanges();
        }

        static void AddSections(Old.Activity oldActivity, New.Activity newActivity)
        {
            New.Section newSection = new New.Section()
            {
                ActivityID = newActivity.ActivityID,
                SectionCode = oldActivity.ActivityCode
            };
            sectionID++;
            newDB.Sections.InsertOnSubmit(newSection);
            newDB.SubmitChanges();

            foreach (Old.Session s in oldActivity.Sessions)
                AddSession(s, newSection);

            // Add the instructors
            foreach (Old.InstructorToActivity a in oldActivity.InstructorToActivities)
            {
                var oldInstResults = from p in oldDB.Instructors
                                  where p.Name == a.Instructor.Name
                                  select p.Name;

                if (oldInstResults == null)
                    continue;

                string instName = oldInstResults.ToList()[0];

                int? instID = (from p in newDB.Instructors
                               where p.Name == instName
                               select p.InstructorID).ToList()[0];

                if (instID == null)
                    continue;

                New.InstructorToActivity newInstToAct = new New.InstructorToActivity()
                {
                    InstructorID = (int) instID,
                    SectionID = sectionID
                };
                newDB.InstructorToActivities.InsertOnSubmit(newInstToAct);
                newDB.SubmitChanges();
            }
        }

        static void ConvertCourseInfo()
        {
            foreach (Old.Course course in oldDB.Courses)
            {
                string code = course.Code;
                string term = course.Title;

                char? convertedTerm = null;
                if (term != null)
                    convertedTerm = term[0];

                string title = course.Title;
                string campus = "St. George";

                List<Old.Activity> lectures = (from p in course.Activities
                                               where p.ActivityType == 'L'
                                               select p).ToList();

                List<Old.Activity> tutorials = (from p in course.Activities
                                                where p.ActivityType == 'T'
                                                select p).ToList();

                List<Old.Activity> practicals = (from p in course.Activities
                                                 where p.ActivityType == 'P'
                                                 select p).ToList();


                New.Course newCourse = new New.Course()
                {
                    Code = code,
                    Term = convertedTerm,
                    Title = title,
                    Description = null,
                    Campus = campus
                };
                courseID++;
                newDB.Courses.InsertOnSubmit(newCourse);
                newDB.SubmitChanges();

                New.Activity newLectureActivity = new New.Activity()
                {
                    CourseID = courseID,
                    ActivityType = 'L'
                };
                activityID++;
                newDB.Activities.InsertOnSubmit(newLectureActivity);
                newDB.SubmitChanges();


                New.Activity newTutorialActivity = new New.Activity()
                {
                    CourseID = courseID,
                    ActivityType = 'T'
                };
                activityID++;
                newDB.Activities.InsertOnSubmit(newTutorialActivity);
                newDB.SubmitChanges();

                New.Activity newPracticalActivity = new New.Activity()
                {
                    CourseID = courseID,
                    ActivityType = 'P'
                };
                activityID++;
                newDB.Activities.InsertOnSubmit(newPracticalActivity);
                newDB.SubmitChanges();


                foreach (Old.Activity a in lectures)
                    AddSections(a, newLectureActivity);

                foreach (Old.Activity a in tutorials)
                    AddSections(a, newTutorialActivity);

                foreach (Old.Activity a in practicals)
                    AddSections(a, newPracticalActivity);
            }
        }

        static void ConvertBuildingsInfo()
        {
            foreach (Old.Building oldB in oldDB.Buildings)
            {
                New.Building newB = new New.Building()
                {
                    BuildingName = oldB.BuildingName,
                    BuildingCode = oldB.BuildingCode,
                    Address = oldB.Address,
                    Latitude = oldB.Latitude,
                    Longitude = oldB.Longitude
                };

                newDB.Buildings.InsertOnSubmit(newB);
                newDB.SubmitChanges();
            }
        }

        static void ConvertBuildingDistances()
        {
            foreach (Old.BuildingDistance oldD in oldDB.BuildingDistances)
            {
                New.Building building1 = (from p in newDB.Buildings
                                          where p.Address == oldD.Building.Address
                                          select p).ToList()[0];
                New.Building building2 = (from p in newDB.Buildings
                                          where p.Address == oldD.Building1.Address
                                          select p).ToList()[0];

                // Put the distances
                New.BuildingDistance newD = new New.BuildingDistance()
                {
                    Building = building1,
                    Building1 = building2,
                    WalkingDuration = oldD.WalkingDuration,
                    WalkingDistance = oldD.WalkingDistance,
                    CyclingDuration = oldD.CyclingDuration,
                    CyclingDistance = oldD.CyclingDistance,
                    TransitDuration = oldD.TransitDuration,
                    TransitDistance = oldD.TransitDistance,
                    DrivingDuration = oldD.VehicleDuration,
                    DrivingDistance = oldD.VehicleDistance
                };

                newDB.BuildingDistances.InsertOnSubmit(newD);
                newDB.SubmitChanges();
            }
        }

        static void ConvertInstructorsList()
        {
            foreach (Old.Instructor oldInstructor in oldDB.Instructors)
            {
                New.Instructor newInstructor = new New.Instructor()
                {
                    Name = oldInstructor.Name,
                    Rating = oldInstructor.Rating
                };
                newDB.Instructors.InsertOnSubmit(newInstructor);
                newDB.SubmitChanges();
            }
        }

        static void Main(string[] args)
        {
            //ConvertInstructorsList();
            //ConvertCourseInfo();
            //ConvertBuildingsInfo();
            ConvertBuildingDistances();

            newDB.Connection.Close();
            oldDB.Connection.Close();
            newDB.Dispose();
            oldDB.Dispose();
        }
    }
}
