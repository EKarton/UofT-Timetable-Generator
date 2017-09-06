using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using OpenQA.Selenium;
using HtmlAgilityPack;

namespace UoftTimetableGenerator.WebScrapper
{
    public class CourseSchedulePopulator
    {
        private UofTDataContext db = new UofTDataContext();
        
        private int sessionID = 0;
        private int instructorID = 0;
        private int sectionID = 0;
        private int activityID = 0;
        private int courseID = 0;

        private int? GetBuildingID(string buildingCode)
        {
            var buildingResults = from p in db.Buildings
                                  where p.BuildingCode == buildingCode
                                  select p.BuildingID;

            if (buildingResults == null || buildingResults.ToList().Count != 1)
                return null;
            else
                return buildingResults.ToList()[0];
        }

        private int GetIndexFromWeekday(string weekday)
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
                    throw new Exception("This type of day has not been handled before!");
            }
        }

        private double? FormatTime(int weekdayIndex, string clockTime)
        {
            try
            {
                string[] clockComp = clockTime.Split(':');
                string hr = clockComp[0].PadLeft(2, '0');
                string min = clockComp[1].PadLeft(2, '0');
                string completeTime = weekdayIndex + hr + "." + min;
                return Convert.ToDouble(completeTime);
            }
            catch { return null; }
        }

        private List<Session> GetYearlongSessions(IWebElement sessionsDiv, IWebElement locsDiv)
        {
            var sessionDivs = sessionsDiv.FindElements(By.ClassName("colDay")).ToList();
            var termLocDivs = locsDiv.FindElements(By.XPath("div[@class='colSectionLocRooms']")).ToList();
            var fallLocDivs = new List<IWebElement>();
            if (termLocDivs.Count > 0)
                fallLocDivs = termLocDivs[0].FindElements(By.ClassName("colDay")).ToList();            
            var winterLocDivs = new List<IWebElement>();
            if (termLocDivs.Count > 1)
                winterLocDivs = termLocDivs[1].FindElements(By.ClassName("colDay")).ToList();

            List<Session> sessions = new List<Session>();
            for (int i = 0; i < sessionDivs.Count; i++)
            {
                // If the times do not exist, then there is no session
                if (sessionDivs[i].FindElement(By.XPath("span[@class='dayInfo']/time[1]")) == null)
                    continue;
                if (sessionDivs[i].FindElement(By.XPath("span[@class='dayInfo']/time[2]")) == null)
                    continue;
                if (sessionDivs[i].FindElement(By.ClassName("weekDay")) == null)
                    continue;

                Session newSession = new Session();

                // Parsing the times
                string weekday = sessionDivs[i].FindElement(By.ClassName("weekDay")).Text.Trim();
                int weekdayIndex = -1;
                if (weekday.Length > 1)
                    weekdayIndex = GetIndexFromWeekday(weekday);

                string startTime_raw = sessionDivs[i].FindElement(By.XPath("span[@class='dayInfo']/time[1]")).Text;
                string endTime_raw = sessionDivs[i].FindElement(By.XPath("span[@class='dayInfo']/time[2]")).Text;
                double? startTime = FormatTime(weekdayIndex, startTime_raw);
                double? endTime = FormatTime(weekdayIndex, endTime_raw);

                // Adding the times
                newSession.StartTime = startTime;
                newSession.EndTime = endTime;

                // Parsing the fall location
                string location_fall = fallLocDivs[i].Text;
                int? buildingID_fall = null;
                string roomNum_fall = null;
                if (i < fallLocDivs.Count && location_fall.Length >= 2)
                {
                    buildingID_fall = GetBuildingID(location_fall.Split(' ')[0].Trim());
                    roomNum_fall = location_fall.Split(' ')[1].Trim();
                }

                // Parsing the winter location
                string location_winter = winterLocDivs[i].Text;
                int? buildingID_winter = null;
                string roomNum_winter = null;
                if (i < winterLocDivs.Count && location_winter.Length >= 2)
                {
                    buildingID_winter = GetBuildingID(location_winter.Split(' ')[0].Trim());
                    roomNum_winter = location_winter.Split(' ')[1].Trim();
                }

                // Adding the location
                newSession.Fall_BuildingID = buildingID_fall;
                newSession.Fall_RoomNumber = roomNum_fall;
                newSession.Winter_BuildingID = buildingID_winter;
                newSession.Winter_RoomNumber = roomNum_winter;

                sessions.Add(newSession);
            }
            return sessions;
        }

        private List<Session> GetTermedSessions(IWebElement sessionsDiv, IWebElement locsDiv, string term)
        {
            var sessionDivs = sessionsDiv.FindElements(By.ClassName("colDay"));
            var locDivs = locsDiv.FindElements(By.ClassName("colDay"));
            List<Session> sessions = new List<Session>();
            for (int i = 0; i < sessionDivs.Count; i++)
            {
                // If the times do not exist, then there is no session
                if (sessionDivs[i].FindElement(By.XPath("span[@class='dayInfo']/time[1]")) == null)
                    continue;
                if (sessionDivs[i].FindElement(By.XPath("span[@class='dayInfo']/time[2]")) == null)
                    continue;
                if (sessionDivs[i].FindElement(By.ClassName("weekDay")) == null)
                    continue;

                Session newSession = new Session();

                // Parsing the times
                string weekday = sessionDivs[i].FindElement(By.ClassName("weekDay")).Text.Trim();
                int weekdayIndex = -1;
                if (weekday.Length > 1)
                    weekdayIndex = GetIndexFromWeekday(weekday);

                string startTime_raw = sessionDivs[i].FindElement(By.XPath("span[@class='dayInfo']/time[1]")).Text;
                string endTime_raw = sessionDivs[i].FindElement(By.XPath("span[@class='dayInfo']/time[2]")).Text;
                double? startTime = FormatTime(weekdayIndex, startTime_raw);
                double? endTime = FormatTime(weekdayIndex, endTime_raw);

                // Adding the times
                newSession.StartTime = startTime;
                newSession.EndTime = endTime;

                // Parsing the location
                string location = locDivs[i].Text;
                int? buildingID = null;
                string roomNum = null;
                if (i < locDivs.Count && location.Length >= 2)
                {
                    buildingID = GetBuildingID(location.Split(' ')[0].Trim());
                    roomNum = location.Split(' ')[1].Trim();
                }

                // Adding the location
                if (term == "F")
                {
                    newSession.Fall_BuildingID = buildingID;
                    newSession.Fall_RoomNumber = roomNum;
                }
                else if (term == "S")
                {
                    newSession.Winter_BuildingID = buildingID;
                    newSession.Winter_RoomNumber = roomNum;
                }

                sessions.Add(newSession);
            }
            return sessions;
        }

        private Instructor GetInstructor(string name)
        {
            return new Instructor()
            {
                Name = name,
                Rating = null
            };
        }

        private Section GetSection(IWebElement sessionContainer, string term)
        {
            // Set the code
            Section section = new Section();
            section.SectionCode = sessionContainer.FindElement(By.ClassName("colCode")).Text;
            
            // Add the instructors
            var instructorElements = sessionContainer.FindElements(By.XPath("td[@class='colInst']/ul/li"));
            foreach (var instructorElement in instructorElements)
            {
                string instructorName = instructorElement.Text.Trim();
                var instructorResults = from p in db.Instructors
                                        where p.Name == instructorName
                                        select p;

                // If it is a new instructor
                if (instructorResults == null || instructorResults.ToList().Count == 0)
                {
                    section.InstructorToSections.Add(new InstructorToSection()
                    {
                        Instructor = GetInstructor(instructorName),
                        Section = section
                    });
                }
                else
                {
                    section.InstructorToSections.Add(new InstructorToSection() {
                        Instructor = instructorResults.ToList()[0],
                        Section = section
                    });
                }
            }

            // Set up the times
            var sessionsDiv = sessionContainer.FindElement(By.XPath("td[@class='colTime']"));
            var locsDiv = sessionContainer.FindElement(By.XPath("td[@class='colLoc']"));
            if (term == "Y")
                section.Sessions.AddRange(GetYearlongSessions(sessionsDiv, locsDiv));
            else if (term == "F" || term == "S")
                section.Sessions.AddRange(GetTermedSessions(sessionsDiv, locsDiv, term));
            else
                throw new Exception("This term was not handled yet!");

            // Return the new object
            return section;
        }

        private Course GetCourses(IWebElement courseTable, string title, string desc, string campus)
        {
            // Create a new course
            Course course = new Course();
            course.Code = courseTable.FindElement(By.XPath("tbody/tr[2]/td/span")).Text;
            course.Term = course.Code[course.Code.Length - 1].ToString()[0];
            course.Description = desc;
            course.Title = title;
            course.Campus = campus;

            // If it already exists in the database
            var existingResults = from p in db.Courses
                                  where p.Code == course.Code
                                  select p;

            if (existingResults != null && existingResults.ToList().Count > 0)
                return null;

            // Get the sections and sort it by type
            List < Section > lectures = new List<Section>();
            List<Section> tutorials = new List<Section>();
            List<Section> practicals = new List<Section>();

            var sectionElements = courseTable.FindElements(By.XPath("tbody/tr[@class='perMeeting']"));
            foreach (var sessionDiv in sectionElements)
            {
                // Get the container that contains all the data of the meeting
                IWebElement sessionContainer = null;
                var possibleSectionDatas = sessionDiv.FindElements(By.XPath("td/table/tbody/tr[@class='sectionData']"));
                foreach (var sectionData in possibleSectionDatas)
                {
                    if (sectionData.FindElements(By.ClassName("colCode")).Count > 0)
                    {
                        sessionContainer = sectionData;
                        break;
                    }
                }

                // Get the sections
                Section section = GetSection(sessionContainer, course.Term.ToString());

                // Sort them
                switch (section.SectionCode.Substring(0, 3))
                {
                    case "LEC":
                        lectures.Add(section);
                        break;
                    case "TUT":
                        tutorials.Add(section);
                        break;
                    case "PRA":
                        practicals.Add(section);
                        break;
                    default:
                        throw new Exception("Non-existant!");
                }
            }

            if (lectures.Count > 0)
            {
                Activity activity = new Activity()
                {
                    ActivityType = 'L',
                };
                activity.Sections.AddRange(lectures);
                course.Activities.Add(activity);
            }
            if (tutorials.Count > 0)
            {
                Activity activity = new Activity()
                {
                    ActivityType = 'T',
                };
                activity.Sections.AddRange(tutorials);
                course.Activities.Add(activity);
            }
            if (practicals.Count > 0)
            {
                Activity activity = new Activity()
                {
                    ActivityType = 'P',
                };
                activity.Sections.AddRange(practicals);
                course.Activities.Add(activity);
            }

            return course;
        }

        private void CreateCourseSchedule(string courseCode, string title, string desc, string campus)
        {
            TimetablePage.ClearFilters();
            TimetablePage.EnterCourseCode(courseCode);
            TimetablePage.SearchForCourses();
            TimetablePage.WaitForContentToLoad();

            // Get course info for both the fall and winter term
            List<Course> coursesInfo = new List<Course>();

            // If there are no courses available then output it
            if (Browser.FindElement("id", "courseSearchResultNum").Text == "0 courses found.")
                Console.WriteLine(courseCode + " has no courses!!!");

            else
            {
                var courseTables = Browser.FindElements("xpath", "//table[@class='perCourse']");
                foreach (var courseTable in courseTables)
                {
                    Course newCourse = GetCourses(courseTable, title, desc, campus);
                    if (newCourse != null)
                        coursesInfo.Add(newCourse);
                }
            }
            db.Courses.InsertAllOnSubmit(coursesInfo);
            db.SubmitChanges();
        }

        private void CreateCourseSchedule()
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(Browser.WebInstance.PageSource);
            var courseTables = htmlDoc.DocumentNode.SelectNodes("//table[@class='perCourse']").ToList();

            /*
            for (int i = 0; i < courseTables.Count; i++)
            {
                var course = GetCourses(courseTables[i], null, null, "St. George");
                if (course != null)
                {
                    db.Courses.InsertOnSubmit(course);
                    db.SubmitChanges();
                }
            }
            */
        }

        public void CreateCourseSchedules(bool overrideAllSchedules)
        {
            db.ObjectTrackingEnabled = true;

            if (overrideAllSchedules)
                RemoveAllCourseSchedules();

            Browser.Initialize();
            TimetablePage.GotoPage();

            /*
            var courses = JsonConvert.DeserializeObject<List<Course>>(File.ReadAllText("courses.json"));
            for (int i = 0; i < courses.Count; i++)
            {

            }

            foreach (Course c in courses)
            {
                var courseResults = from p in db.Courses
                                    where p.Code == c.Code
                                    select p;

                if (courseResults != null && courseResults.ToList().Count > 0)
                    continue;

                CreateCourseSchedule(c.Code, c.Title, c.Description, c.Campus);
            }
            */

            TimetablePage.SelectTerm(new string[] { "F", "S", "Y" });
            TimetablePage.SearchForCourses();
            TimetablePage.WaitForContentToLoad();

            CreateCourseSchedule();

            Browser.Close();
            db.Connection.Close();
            db.Dispose();
        }

        private void RemoveAllCourseSchedules()
        {
            // Delete all entities
            db.ExecuteCommand("DELETE FROM Session");
            db.ExecuteCommand("DELETE FROM InstructorToSection");
            db.ExecuteCommand("DELETE FROM Instructor");
            db.ExecuteCommand("DELETE FROM Section");
            db.ExecuteCommand("DELETE FROM Activity");
            db.ExecuteCommand("DELETE FROM Course");

            // Reset the primary key
            db.ExecuteCommand("DBCC CHECKIDENT ('Session', RESEED, 0)");
            db.ExecuteCommand("DBCC CHECKIDENT ('InstructorToSection', RESEED, 0)");
            db.ExecuteCommand("DBCC CHECKIDENT ('Instructor', RESEED, 0)");
            db.ExecuteCommand("DBCC CHECKIDENT ('Section', RESEED, 0)");
            db.ExecuteCommand("DBCC CHECKIDENT ('Activity', RESEED, 0)");
            db.ExecuteCommand("DBCC CHECKIDENT ('Course', RESEED, 0)");
        }
    }
}
