using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.DataModels
{
    internal class CourseFactory
    {
        /// <summary>
        /// Creates a DataModels.Course object from DataContext.Course object
        /// </summary>
        /// <param name="oldCourse">Raw data about a course from the database</param>
        public static Course GetCourse(DataContext.Course oldCourse)
        {
            Course newCourse = new Course()
            {
                CourseCode = oldCourse.Code,
                Title = oldCourse.Title,
                Term = oldCourse.Term.ToString(),
                Description = oldCourse.Description,
                Campus = oldCourse.Campus,
                Activities = new List<Activity>()
            };

            foreach (DataContext.Activity oldActivity in oldCourse.Activities)
                newCourse.Activities.Add(GetActivity(oldActivity, newCourse));

            return newCourse;
        }

        /// <summary>
        /// Creates an DataModels.Activity object from DataContext.Activity
        /// </summary>
        /// <param name="oldActivity">The raw activity data from the database</param>
        /// <param name="course">The course the activity belongs to</param>
        private static Activity GetActivity(DataContext.Activity oldActivity, Course course)
        {
            Activity activity = new Activity();
            activity.Course = course;

            if (oldActivity.ActivityType == null)
                activity.ActivityType = "";
            else
                activity.ActivityType = oldActivity.ActivityType.ToString();

            activity.Sections = new List<Section>();
            foreach (DataContext.Section oldSection in oldActivity.Sections)
                activity.Sections.Add(GetSection(oldSection, activity));

            return activity;
        }

        /// <summary>
        /// Creates a DataModels.Section object from a DataContext.Section object
        /// </summary>
        /// <param name="oldSection">The section data from the database</param>
        /// <param name="activity">The activity this section is associated with</param>
        private static Section GetSection(DataContext.Section oldSection, Activity activity)
        {
            Section section = new Section()
            {
                Activity = activity,
                SectionCode = oldSection.SectionCode,
                Instructors = new List<string>(),
                Sessions = new List<Session>()
            };
            foreach (DataContext.Session oldSession in oldSection.Sessions)
                section.Sessions.Add(GetSession(oldSession, section));
            return section;
        }

        /// <summary>
        /// Creates a DataModels.Session object from a DataContext.Session object
        /// </summary>
        /// <param name="oldSession">The raw session data from the database</param>
        /// <param name="sectionAttachedTo">The section that this session belongs to</param>
        private static Session GetSession(DataContext.Session oldSession, Section sectionAttachedTo)
        {
            Session session = new Session();
            session.Section = sectionAttachedTo;

            if (oldSession.Building == null)
                session.FallBuilding = null;
            else
                session.FallBuilding = new Building(oldSession.Building);

            if (oldSession.Building1 == null)
                session.WinterBuilding = null;
            else
                session.WinterBuilding = new Building(oldSession.Building1);

            session.FallRoomNumber = oldSession.Fall_RoomNumber;
            session.WinterRoomNumber = oldSession.Winter_RoomNumber;
            session.StartTimeWithWeekday = oldSession.StartTime.GetValueOrDefault(0);
            session.EndTimeWithWeekday = oldSession.EndTime.GetValueOrDefault(0);

            return session;
        }
    }
}
