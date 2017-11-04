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
    /// <summary>
    /// Populates the database with course data coming from the web
    /// </summary>
    public class CourseSchedulePopulator
    {
        private UofTDataContext db = new UofTDataContext();

        /// <summary>
        /// Get the building id of a particular building stored in the database
        /// It returns null if there is no building found with a particular building code or 
        /// there is more than one building with that particular building code
        /// </summary>
        /// <param name="buildingCode">The building code</param>
        /// <returns>The building id.</returns>
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

        /// <summary>
        /// Get the index to a particular weekday
        /// </summary>
        /// <param name="weekday">The weekday</param>
        /// <returns>The index to that weekday</returns>
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

        /// <summary>
        /// Formats a time into a compressed time containing the weekday index and the 24-hr time in one double
        /// </summary>
        /// <param name="weekdayIndex">The index to a weekday</param>
        /// <param name="clockTime">The clock time (hr:min) in 24 hr time</param>
        /// <returns>The compressed time</returns>
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

        /// <summary>
        /// Get a list of sessions associated to a particular section of a year-long course
        /// </summary>
        /// <param name="sessionsDiv">A container containing all the sessions</param>
        /// <param name="locsDiv">A container containing all the locations to all the sessions</param>
        /// <returns>A list of sessions</returns>
        private List<Session> GetYearlongSessions(HtmlNode sessionsDiv, HtmlNode locsDiv)
        {
            var rawSessionDivs = sessionsDiv.SelectNodes(".//*[@class='colDay']");
            var sessionDivs = new List<HtmlNode>();
            if (rawSessionDivs != null)
                sessionDivs = rawSessionDivs.ToList();

            var rawTermLocDivs = locsDiv.SelectNodes("div[@class='colSectionLocRooms']");
            var termLocDivs = new List<HtmlNode>();
            if (rawTermLocDivs != null)
                termLocDivs = rawTermLocDivs.ToList();

            var fallLocDivs = new List<HtmlNode>();
            if (termLocDivs.Count > 0)
            {
                var rawFallLocDivs = termLocDivs[0].SelectNodes(".//*[@class='colDay']");
                if (rawFallLocDivs != null)
                    fallLocDivs = rawFallLocDivs.ToList();
            }
            var winterLocDivs = new List<HtmlNode>();
            if (termLocDivs.Count > 1)
            {
                var rawWinterLocDivs = termLocDivs[1].SelectNodes(".//*[@class='colDay']");
                if (rawWinterLocDivs != null)
                    winterLocDivs = rawWinterLocDivs.ToList();
            }

            List<Session> sessions = new List<Session>();
            for (int i = 0; i < sessionDivs.Count; i++)
            {
                // If the times do not exist, then there is no session
                if (sessionDivs[i].SelectSingleNode("span[@class='dayInfo']/time[1]") == null)
                    continue;
                if (sessionDivs[i].SelectSingleNode("span[@class='dayInfo']/time[2]") == null)
                    continue;
                if (sessionDivs[i].SelectSingleNode(".//*[@class='weekDay']") == null)
                    continue;

                Session newSession = new Session();

                // Parsing the times
                string weekday = sessionDivs[i].SelectSingleNode(".//*[@class='weekDay']").InnerText.Replace("\n", "").Replace("\r", "").Trim();
                int weekdayIndex = -1;
                if (weekday.Length > 1)
                    weekdayIndex = GetIndexFromWeekday(weekday);

                string startTime_raw = sessionDivs[i].SelectSingleNode("span[@class='dayInfo']/time[1]").InnerText.Replace("\n", "").Replace("\r", "").Trim();
                string endTime_raw = sessionDivs[i].SelectSingleNode("span[@class='dayInfo']/time[2]").InnerText.Replace("\n", "").Replace("\r", "").Trim();
                double? startTime = FormatTime(weekdayIndex, startTime_raw);
                double? endTime = FormatTime(weekdayIndex, endTime_raw);

                // Adding the times
                newSession.StartTime = startTime;
                newSession.EndTime = endTime;

                // Parsing the fall location
                string location_fall = fallLocDivs[i].InnerText.Replace("\n", "").Replace("\r", "").Trim();
                int? buildingID_fall = null;
                string roomNum_fall = null;
                if (i < fallLocDivs.Count && location_fall.Length >= 2)
                {
                    buildingID_fall = GetBuildingID(location_fall.Split(' ')[0].Trim());
                    roomNum_fall = location_fall.Split(' ')[1].Trim();
                }

                // Parsing the winter location
                string location_winter = winterLocDivs[i].InnerText.Replace("\n", "").Replace("\r", "").Trim();
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

        /// <summary>
        /// Get a list of sessions for a particular section of a termed course
        /// </summary>
        /// <param name="sessionsDiv">The container containing all the sessions</param>
        /// <param name="locsDiv">The container containing all the locations to each session</param>
        /// <param name="term">The term of the course</param>
        /// <returns>A list of sessions</returns>
        private List<Session> GetTermedSessions(HtmlNode sessionsDiv, HtmlNode locsDiv, string term)
        {
            var rawSessionDivs = sessionsDiv.SelectNodes(".//*[@class='colDay']");
            var sessionDivs = new List<HtmlNode>();
            if (rawSessionDivs != null)
                sessionDivs = rawSessionDivs.ToList();

            var rawLocDivs = locsDiv.SelectNodes(".//*[@class='colDay']");
            var locDivs = new List<HtmlNode>();
            if (rawLocDivs != null)
                locDivs = rawLocDivs.ToList();

            List<Session> sessions = new List<Session>();
            for (int i = 0; i < sessionDivs.Count; i++)
            {
                // If the times do not exist, then there is no session
                if (sessionDivs[i].SelectSingleNode("span[@class='dayInfo']/time[1]") == null)
                    continue;
                if (sessionDivs[i].SelectSingleNode("span[@class='dayInfo']/time[2]") == null)
                    continue;
                if (sessionDivs[i].SelectSingleNode(".//*[@class='weekDay']") == null)
                    continue;

                Session newSession = new Session();

                // Parsing the times
                string weekday = sessionDivs[i].SelectSingleNode(".//*[@class='weekDay']").InnerText.Replace("\n", "").Replace("\r", "").Trim();
                int weekdayIndex = -1;
                if (weekday.Length > 1)
                    weekdayIndex = GetIndexFromWeekday(weekday);

                string startTime_raw = sessionDivs[i].SelectSingleNode("span[@class='dayInfo']/time[1]").InnerText.Replace("\n", "").Replace("\r", "").Trim();
                string endTime_raw = sessionDivs[i].SelectSingleNode("span[@class='dayInfo']/time[2]").InnerText.Replace("\n", "").Replace("\r", "").Trim();
                double? startTime = FormatTime(weekdayIndex, startTime_raw);
                double? endTime = FormatTime(weekdayIndex, endTime_raw);

                // Adding the times
                newSession.StartTime = startTime;
                newSession.EndTime = endTime;

                // Parsing the location
                string location = locDivs[i].InnerText.Replace("\n", "").Replace("\r", "").Trim();
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

        /// <summary>
        /// Creates a new instructor object
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Instructor GetInstructor(string name)
        {
            return new Instructor()
            {
                Name = name,
                Rating = null
            };
        }

        /// <summary>
        /// Gets the sections of the course
        /// </summary>
        /// <param name="sessionContainer">The container containing the section data</param>
        /// <param name="term">The term</param>
        /// <returns>The section</returns>
        private Section GetSection(HtmlNode sessionContainer, string term)
        {
            // Set the code
            Section section = new Section();
            section.SectionCode = sessionContainer.SelectSingleNode(".//*[@class='colCode']").InnerText.Replace("\n", "").Replace("\r", "").Trim();
            
            // Add the instructors
            var rawInstructorElements = sessionContainer.SelectNodes("td[@class='colInst']/ul/li");
            var instructorElements = new List<HtmlNode>();
            if (rawInstructorElements != null)
                instructorElements = rawInstructorElements.ToList();

            foreach (var instructorElement in instructorElements)
            {
                string instructorName = instructorElement.InnerText.Replace("\n", "").Replace("\r", "").Trim();
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
                    section.InstructorToSections.Add(new InstructorToSection()
                    {
                        InstructorID = instructorResults.ToList()[0].InstructorID,
                        Section = section
                    });
                }
            }

            // Set up the times
            var sessionsDiv = sessionContainer.SelectSingleNode("td[@class='colTime']");
            var locsDiv = sessionContainer.SelectSingleNode("td[@class='colLoc']");
            if (term == "Y")
                section.Sessions.AddRange(GetYearlongSessions(sessionsDiv, locsDiv));
            else if (term == "F" || term == "S")
                section.Sessions.AddRange(GetTermedSessions(sessionsDiv, locsDiv, term));
            else
                throw new Exception("This term was not handled yet!");

            // Return the new object
            return section;
        }

        /// <summary>
        /// Parses the html and get the courses
        /// </summary>
        /// <param name="courseTable">The html table, containing the course</param>
        /// <param name="title">The title of the course</param>
        /// <param name="desc">The description of the course</param>
        /// <param name="campus">The campus of the course</param>
        /// <returns>The course object</returns>
        private Course GetCourses(HtmlNode courseTable, string title, string desc, string campus)
        {
            // Create a new course
            Course course = new Course();
            course.Code = courseTable.SelectSingleNode("tbody/tr[2]/td/span").InnerText.Replace("\n", "").Replace("\r", "").Trim();
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

            var rawSectionElements = courseTable.SelectNodes("tbody/tr[@class='perMeeting']");
            var sectionElements = new List<HtmlNode>();
            if (rawSectionElements != null)
                sectionElements = rawSectionElements.ToList();

            foreach (var sessionDiv in sectionElements)
            {
                // Get the container that contains all the data of the meeting
                HtmlNode sessionContainer = null;
                var rawPossibleSectionDatas = sessionDiv.SelectNodes("td/table/tbody/tr[@class='sectionData']");
                var possibleSectionDatas = new List<HtmlNode>();
                if (rawPossibleSectionDatas != null)
                    possibleSectionDatas = rawPossibleSectionDatas.ToList();

                foreach (var sectionData in possibleSectionDatas)
                {
                    var rawSessionData = sectionData.SelectNodes(".//*[@class='colCode']");
                    if (rawSessionData != null && rawSessionData.ToList().Count > 0)
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

        /// <summary>
        /// Parses the html and puts the new courses into the database
        /// </summary>
        /// <param name="courseCode">The course code</param>
        /// <param name="title">The title of the course</param>
        /// <param name="desc">The description of the course</param>
        /// <param name="campus">The campus</param>
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
                    //Course newCourse = GetCourses(courseTable, title, desc, campus);
                    Course newCourse = null;
                    if (newCourse != null)
                        coursesInfo.Add(newCourse);
                }
            }
            db.Courses.InsertAllOnSubmit(coursesInfo);
            db.SubmitChanges();
        }

        /// <summary>
        /// Parses the html and populate the courses that has not been placed in the database before
        /// </summary>
        private void CreateCourseSchedule()
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(Browser.WebInstance.PageSource);
            var courseTables = htmlDoc.DocumentNode.SelectNodes("//table[@class='perCourse']").ToList();

            for (int i = 0; i < courseTables.Count; i++)
            {
                var course = GetCourses(courseTables[i], null, null, "St. George");
                if (course != null)
                {
                    db.Courses.InsertOnSubmit(course);
                    db.SubmitChanges();
                }
            }
        }

        /// <summary>
        /// Parses the HTML and populates the courses in the database
        /// If {overrideAllSchedules} is true, it will delete all existing course data from the database
        /// </summary>
        /// <param name="overrideAllSchedules">Whether to delete existing course data or not</param>
        public void CreateCourseSchedules(bool overrideAllSchedules)
        {
            db.ObjectTrackingEnabled = true;

            if (overrideAllSchedules)
                RemoveAllCourseSchedules();

            Browser.Initialize();
            TimetablePage.GotoPage();
            TimetablePage.SelectTerm(new string[] { "F", "S", "Y" });
            TimetablePage.SearchForCourses();
            TimetablePage.WaitForContentToLoad();

            CreateCourseSchedule();

            Browser.Close();
            db.Connection.Close();
            db.Dispose();
        }

        /// <summary>
        /// Deletes all existing course data from the database
        /// </summary>
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
