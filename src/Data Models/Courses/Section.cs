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
        /// <param name="oldSection">The section data from the database</param>
        /// <param name="activity">The activity this section is associated with</param>
        public Section(DataContext.Section oldSection, Activity activity)
        {
            Activity = activity;
            SectionCode = oldSection.SectionCode;
            Instructors = new List<string>();
            Sessions = new List<Session>();
            foreach (DataContext.Session oldSession in oldSection.Sessions)
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