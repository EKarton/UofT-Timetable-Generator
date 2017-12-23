using System.Collections.Generic;

namespace UoftTimetableGenerator.DataModels
{
    /// <summary>
    /// A class used to represent an activity
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// A constructor used to convert the DataContext.Activity to Datamodels.Activity
        /// </summary>
        /// <param name="oldActivity">The raw activity data from the database</param>
        /// <param name="course">The course the activity belongs to</param>
        internal Activity(DataContext.Activity oldActivity, Course course)
        {
            Course = course;

            if (oldActivity.ActivityType == null)
                ActivityType = "";
            else
                ActivityType = oldActivity.ActivityType.ToString();

            Sections = new List<Section>();
            foreach (DataContext.Section oldSection in oldActivity.Sections)
                Sections.Add(new Section(oldSection, this));
        }

        /// <summary>
        /// Get / set the course that this activity belongs to
        /// </summary>
        public Course Course { get; set; }

        /// <summary>
        /// Get / set the activity type
        /// </summary>
        public string ActivityType { get; set; }

        /// <summary>
        /// Get / set the sections available in this activity
        /// </summary>
        public List<Section> Sections { get; set; }
    }
}