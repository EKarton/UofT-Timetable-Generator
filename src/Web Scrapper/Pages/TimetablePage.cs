using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.WebScrapper
{
    internal static class TimetablePage
    {
        /*
        private static int? GetBuildingID(string buildingCode)
        {
            using (UofTDataContext db = new UofTDataContext())
            {
                var buildingResults = from p in db.Buildings
                                      where p.BuildingCode == buildingCode
                                      select p.BuildingID;

                if (buildingResults == null || buildingResults.ToList().Count != 1)
                    return null;
                else
                    return buildingResults.ToList()[0];
            }
        }

        private static double FormatTime(string text)
        {
            text = text.Replace(':', '.');
            try
            {
                return Convert.ToDouble(text);
            }
            catch { return 0.00; }
        }

        // We know that the timeDiv is @class colTime and the locationDiv is @class colLoc
        private static Session[] GetTimesAndLocation_YearCourse(IWebElement timesDiv, IWebElement locationDiv)
        {
            List<Session> times = new List<Session>();
            for (int i = 1; i <= timesDiv.FindElements(By.ClassName("colDay")).Count; i++)
            {
                // Getting the correct container that holds the dates and the times
                var timeDiv = timesDiv.FindElement(By.XPath("ul/li[" + i + "]"));

                Session newTime = new Session()
                {
                    Day = timeDiv.FindElement(By.ClassName("weekDay")).Text,
                    StartTime = FormatTime(timeDiv.FindElement(By.XPath("span[@class=\"dayInfo\"]/time[1]")).Text),
                    EndTime = FormatTime(timeDiv.FindElement(By.XPath("span[@class=\"dayInfo\"]/time[2]")).Text),
                    FallLocation = locationDiv.FindElement(By.XPath("div[@class=\"colSectionLocRooms\"][1]/ul/li[" + i + "]")).Text,
                    WinterLocation = locationDiv.FindElement(By.XPath("div[@class=\"colSectionLocRooms\"][2]/ul/li[" + i + "]")).Text
                };
                times.Add(newTime);
            }
            return times.ToArray();
        }

        private static Session[] GetTimesAndLocation_SemesterCourse(IWebElement timesDiv, IWebElement locationDiv, string term)
        {
            List<Session> times = new List<Session>();
            for (int i = 1; i <= timesDiv.FindElements(By.ClassName("colDay")).Count; i++)
            {
                // Getting the correct container that holds the dates and the times
                var timeDiv = timesDiv.FindElement(By.XPath("ul/li[" + i + "]"));

                Session newTime = new Session();
                newTime.Day = timeDiv.FindElement(By.ClassName("weekDay")).Text;
                newTime.StartTime = FormatTime(timeDiv.FindElement(By.XPath("span[@class=\"dayInfo\"]/time[1]")).Text);
                newTime.EndTime = FormatTime(timeDiv.FindElement(By.XPath("span[@class=\"dayInfo\"]/time[2]")).Text);
                string location = locationDiv.FindElement(By.XPath("ul/li[" + i + "]")).Text;

                // Placing the location at the correct property
                if (term == "F")
                    newTime.FallLocation = location;
                else if (term == "S")
                    newTime.WinterLocation = location;
                else
                    throw new Exception("This term was not handled yet!! (2)");

                times.Add(newTime);
            }
            return times.ToArray();
        }

        private static Activity GetMeetingSession(IWebElement sessionContainer, string term)
        {
            // Set the code and instructor of the meeting session
            Activity meetingSession = new Activity();
            meetingSession.Code = sessionContainer.FindElement(By.ClassName("colCode")).Text;
            meetingSession.Instructor = sessionContainer.FindElement(By.ClassName("colInst")).Text;

            // Set up the times
            IWebElement timeDiv = sessionContainer.FindElement(By.ClassName("colTime"));
            IWebElement locDiv = sessionContainer.FindElement(By.ClassName("colLoc"));
            if (term == "Y")
                meetingSession.Sessions = GetTimesAndLocation_YearCourse(timeDiv, locDiv);
            else if (term == "F" || term == "S")
                meetingSession.Sessions = GetTimesAndLocation_SemesterCourse(timeDiv, locDiv, term);
            else
                throw new Exception("This term was not handled yet!");

            // Return the new object
            return meetingSession;
        }

        private static Course GetCourseInfo(IWebElement courseContainer)
        {
            Course courseInfo = new Course();
            courseInfo.CourseName = courseContainer.FindElement(By.XPath("table/tbody/tr[2]/td/span")).Text;
            courseInfo.Term = courseInfo.CourseName[courseInfo.CourseName.Length - 1].ToString();

            List<Activity> lectures = new List<Activity>();
            List<Activity> tutorials = new List<Activity>();
            List<Activity> practicals = new List<Activity>();

            foreach (var sessionDiv in courseContainer.FindElements(By.XPath("table/tbody/tr[@class=\"perMeeting\"]")))
            {
                // Get the container that contains all the data of the meeting
                IWebElement sessionContainer = null;
                foreach (var sectionData in sessionDiv.FindElements(By.XPath("td/table/tbody/tr[@class=\"sectionData\"]")))
                    if (sectionData.FindElements(By.ClassName("colCode")).Count > 0)
                    {
                        sessionContainer = sectionData;
                        break;
                    }

                Activity meetSession = GetMeetingSession(sessionContainer, courseInfo.Term);

                // Sort them
                switch (meetSession.Code.Substring(0, 3))
                {
                    case "LEC":
                        lectures.Add(meetSession);
                        break;
                    case "TUT":
                        tutorials.Add(meetSession);
                        break;
                    case "PRA":
                        practicals.Add(meetSession);
                        break;
                }
            }

            courseInfo.Lectures = lectures.ToArray();
            courseInfo.Tutorials = tutorials.ToArray();
            courseInfo.Practicals = practicals.ToArray();
            return courseInfo;
        }

        public static List<DataModels.Course> GetCourseInfo(string courseName)
        {
            ClearFilters();
            EnterCourseCode(courseName);
            SearchForCourses();

            // Get course info for both the fall and winter term
            List<Course> coursesInfo = new List<Course>();
            try
            {
                // If there are no courses available then throw an exception
                if (Browser.FindElement("id", "courseSearchResultNum").Text == "0 courses found.")
                {
                    Console.WriteLine(courseName + " has no courses!!!");
                    return new List<Course>();
                }

                IWebElement table = Browser.FindElement("id", "courses");
                foreach (var courseContainer in table.FindElements(By.XPath("div[@class=\"courseResults\"]")))
                    coursesInfo.Add(GetCourseInfo(courseContainer));
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + courseName);
                coursesInfo.Add(new Course()
                {
                    CourseName = courseName,
                    Term = ("ERROR HAS OCCURED!" + e.StackTrace)
                });
            }
            return coursesInfo;
        }
        */

        public static void GotoPage()
        {
            Browser.WebInstance.Url = @"https://timetable.iit.artsci.utoronto.ca/";
        }

        public static void WaitForContentToLoad()
        {
            // If the element does not exist, skip it.
            if (Browser.DoesElementExist("id", "jsspinner") == false)
                return;

            // Make the timeout longer
            TimeSpan prevTimeout = Browser.WaitInstance.Timeout;
            TimeSpan prevPollInterval = Browser.WaitInstance.PollingInterval;
            Browser.WaitInstance.Timeout = TimeSpan.FromSeconds(120);
            Browser.WaitInstance.PollingInterval = TimeSpan.FromMilliseconds(100);

            // Wait until the page finished loading
            try
            {
                Browser.WaitInstance.Until(delegate (IWebDriver driver)
                {
                    return !driver.FindElement(By.Id("jsspinner")).Displayed;
                });

                // Reset the timeouts back to default
                Browser.WaitInstance.Timeout = prevTimeout;
                Browser.WaitInstance.PollingInterval = prevPollInterval;
            }
            catch { throw new Exception("Content took too long to load!"); }
        }

        public static void SearchForCourses()
        {
            Browser.FindClickableElement("id", "searchButton").Click();
        }

        public static void EnterCourseCode(string courseID)
        {
            Browser.FindElement("id", "courseCode").SendKeys(courseID);
        }

        public static void SelectTerm(string[] terms)
        {
            Browser.FindClickableElement("xpath", "//*[@id='js-modal-page']/div/div[2]/div[1]/div[3]/div[1]/div[3]/div[2]/div[1]").Click();

            foreach (string term in terms)
            {
                var dropdownOptions = Browser.FindElements("xpath", "//*[@id='js-modal-page']/div/div[2]/div[1]/div[3]/div[1]/div[3]/div[2]/div[2]/div/div");
                foreach (IWebElement option in dropdownOptions)
                {
                    if (option.Text == term)
                    {
                        option.Click();
                        break;
                    }
                }
            }

        }

        public static void ClearFilters()
        {
            Browser.FindClickableElement("id", "btnClear").Click();
        }
    }
}
