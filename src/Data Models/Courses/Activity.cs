using System.Collections.Generic;

namespace UoftTimetableGenerator.DataModels
{
    /// <summary>
    /// A class used to represent an activity
    /// </summary>
    public class Activity
    {
        public Activity()
        {
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