using System.Collections.Generic;

namespace UoftTimetableGenerator.DataModels
{
    /// <summary>
    /// A class representing a section of a course
    /// </summary>
    public class Section
    {
        /// <summary>
        /// Creates an empty section
        /// </summary>
        public Section()
        {
            Activity = null;
            SectionCode = "";
            Instructors = new List<string>();
            Sessions = new List<Session>();
        }

        /// <summary>
        /// Creates a section based on raw data from the database
        /// </summary>
        /// <param name="oldActivity">The activity data from the database</param>
        /// <param name="activityAttachedTo">The new activity which this section is associated with</param>
        public Section(DataContext.Activity oldActivity, Activity activityAttachedTo)
        {
            Activity = activityAttachedTo;
            SectionCode = oldActivity.ActivityCode;
            Instructors = new List<string>();
            Sessions = new List<Session>();
            foreach (DataContext.Session oldSession in oldActivity.Sessions)
                Sessions.Add(new Session(oldSession, this));
        }

        /// <summary>
        /// Get / set the activity the section belongs to
        /// </summary>
        public Activity Activity { get; set; }

        /// <summary>
        /// Get / set the section code of this section
        /// </summary>
        public string SectionCode { get; set; }

        /// <summary>
        /// Get / set a list of instructors that will be teaching in this section
        /// </summary>
        public List<string> Instructors { get; set; }

        /// <summary>
        /// Get / set a list of sessions this section is comprised of
        /// </summary>
        public List<Session> Sessions { get; set; }
    }
}