using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UoftTimetableGenerator.DataModels;
using Newtonsoft.Json;
using System.IO;
using UoftTimetableGenerator.DataContext;

namespace UoftTimetableGenerator.WebScrapper
{
    internal class WebScrapperRunner
    {
        /*
        static int GetIndexFromWeekday(string weekday)
        {
            switch (weekday)
            {
                case "Sunday":
                    return 0;
                case "Monday":
                    return 1;
                case "Tuesday":
                    return 2;
                case "Wednesday":
                    return 3;
                case "Thursday":
                    return 4;
                case "Friday":
                    return 5;
                case "Saturday":
                    return 6;
                default:
                    throw new Exception("This day cannot be determined!");
            }
        }

        static double? ConvertDayAndTimeToSingular(double time, string weekday)
        {
            try
            {
                string[] tokenizedTime = time.ToString().Split('.');
                string hour = tokenizedTime[0].PadLeft(2, '0');

                string minute = "00";
                if (tokenizedTime.Length > 1)
                    minute = tokenizedTime[1].PadRight(2, '0');

                string day = GetIndexFromWeekday(weekday.ToString()).ToString();
                string totalStartTime = day + hour + "." + minute;
                return Convert.ToDouble(totalStartTime);
            }
            catch
            {
                return null;
            }
        }

        static Tuple<int?, string> GetLocationInfo(string rawLocation)
        {
            try
            {
                string[] tokenized = rawLocation.Split(' ');
                string buildingCode = tokenized[0].Trim();
                string room = tokenized[1].Trim();

                int? buildingID = null;
                using (UoftDataContext bdb = new UoftDataContext())
                {
                    var buildingIDs = from row in bdb.Buildings
                                 where row.BuildingCode == buildingCode
                                 select row.Id;

                    if (buildingIDs.ToArray().Length == 1)
                        buildingID = buildingIDs.ToArray()[0];
                }

                return new Tuple<int?, string>(buildingID, room);
            }
            catch
            {
                return new Tuple<int?, string>(null, null);
            }
        }

        static Session GetActivitySession(Time time)
        {
            Tuple<int?, string> fallLoc = GetLocationInfo(time.FallLocation);
            Tuple<int?, string> winterLoc = GetLocationInfo(time.WinterLocation);

            Session newSession = new Session();
            newSession.StartTime = ConvertDayAndTimeToSingular(time.StartTime, time.Day);
            newSession.EndTime = ConvertDayAndTimeToSingular(time.EndTime, time.Day);
            newSession.Fall_BuildingID = fallLoc.Item1;
            newSession.Fall_RoomNumber = fallLoc.Item2;
            newSession.Winter_BuildingID = winterLoc.Item1;
            newSession.Winter_RoomNumber = winterLoc.Item2;

            return newSession;
        }

        static Activity GetCourseActivity(MeetingSession activity, char? activityType)
        {
            Activity newActivity = new Activity();
            newActivity.ActivityCode = activity.MeetingCode;
            newActivity.ActivityType = activityType;

            // Handle the times
            foreach (Time time in activity.Times)
                newActivity.Sessions.Add(GetActivitySession(time));

            return newActivity;
        }

        static void PopulateCourseList()
        {
            using (UoftDataContext cbd = new UoftDataContext())
            {
                CourseList list = JsonConvert.DeserializeObject<CourseList>(File.ReadAllText("Course List.json"));
                foreach (string courseCode in list.Courses)
                {
                    Console.WriteLine(courseCode);
                    string[] filenames = Directory.GetFiles("Course Info", courseCode + "*");
                    foreach (string filename in filenames)
                    {
                        DataModels.Course course = JsonConvert.DeserializeObject<DataModels.Course>(File.ReadAllText(filename));
                        string courseCode_Full = course.CourseName;
                        char? term = course.Term[0];

                        Course newCourse = new Course();
                        newCourse.Code = courseCode_Full;
                        newCourse.Term = term;

                        // Go through each lecture, practical, and tutorial
                        foreach (MeetingSession lecture in course.Lectures)
                            newCourse.Activities.Add(GetCourseActivity(lecture, 'L'));

                        foreach (MeetingSession tutorial in course.Tutorials)
                            newCourse.Activities.Add(GetCourseActivity(tutorial, 'T'));

                        foreach (MeetingSession practical in course.Practicals)
                            newCourse.Activities.Add(GetCourseActivity(practical, 'P'));

                        cbd.Courses.InsertOnSubmit(newCourse);
                    }
                }
                cbd.SubmitChanges();
            }
        }

        static void HandleInstructors(UoftDataContext cbd, int courseID, MeetingSession session, char? type)
        {
            Activity activity = (from row in cbd.Activities
                              where row.CourseID == courseID && row.ActivityCode == session.MeetingCode && row.ActivityType == type
                              select row).ToArray()[0];

            string[] tokenized = session.Instructor.Split('.');
            foreach (string rawInstructor in tokenized)
            {
                string instructor = rawInstructor.Trim();
                if (instructor.Length <= 1)
                    continue;

                var existingInstructors = from row in cbd.Instructors
                                          where row.Name == instructor
                                          select row;

                if (existingInstructors.Count() == 0)
                {
                    Console.WriteLine("Will insert " + instructor);

                    Instructor newInstructor = new Instructor();
                    newInstructor.Name = instructor;
                    newInstructor.Rating = null;
                    cbd.Instructors.InsertOnSubmit(newInstructor);
                    cbd.SubmitChanges();

                    InstructorToActivity newInstructorToActivity = new InstructorToActivity();
                    newInstructorToActivity.Activity = activity;
                    newInstructorToActivity.Instructor = newInstructor;

                    cbd.InstructorToActivities.InsertOnSubmit(newInstructorToActivity);
                    cbd.SubmitChanges();
                    Console.WriteLine("Has inserted " + instructor);
                }
                else
                {
                    Instructor preInstructor = existingInstructors.ToArray()[0];

                    InstructorToActivity newInstructorToActivity = new InstructorToActivity();
                    newInstructorToActivity.Activity = activity;
                    newInstructorToActivity.Instructor = preInstructor;

                    cbd.InstructorToActivities.InsertOnSubmit(newInstructorToActivity);
                    cbd.SubmitChanges();
                    Console.WriteLine("Has inserted " + activity.Id + ", " + instructor);
                }
            }
        }

        static void PopulateInstructorsList()
        {
            using (UoftDataContext cbd = new UoftDataContext())
            {
                CourseList list = JsonConvert.DeserializeObject<CourseList>(File.ReadAllText("Course List.json"));
                foreach (string courseCode in list.Courses)
                {
                    Console.WriteLine(courseCode);
                    string[] filenames = Directory.GetFiles("Course Info", courseCode + "*");
                    foreach (string filename in filenames)
                    {
                        DataModels.Course course = JsonConvert.DeserializeObject<DataModels.Course>(File.ReadAllText(filename));
                        int courseID = (from item in cbd.Courses
                                        where item.Code == course.CourseName && item.Term == course.Term[0]
                                        select item.Id).ToArray()[0];

                        // Go through each lecture, practical, and tutorial
                        foreach (MeetingSession lecture in course.Lectures)
                            HandleInstructors(cbd, courseID, lecture, 'L');

                        foreach (MeetingSession tutorial in course.Tutorials)
                            HandleInstructors(cbd, courseID, tutorial, 'T');

                        foreach (MeetingSession practical in course.Practicals)
                            HandleInstructors(cbd, courseID, practical, 'P');
                    }
                }
            }
        }
        */

        static void Main(string[] args)
        {
            //IWebScrapper a = new BuildingsListScrapper();
            //a.Run();
            //PopulateCourseList();
            //PopulateInstructorsList();
            //IWebScrapper b = new BuildingDistancesScrapper();
            //b.Run();
            BuildingDistancePopulator b = new BuildingDistancePopulator();
            b.InsertBuildingDistancesToDatabase(false);
            Console.ReadKey();
        }
    }
}
