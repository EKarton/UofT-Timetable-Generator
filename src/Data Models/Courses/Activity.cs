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
        /// <param name="oldActivitiesOfSameType">The raw activity data from the database</param>
        /// <param name="activityType">The activity type</param>
        /// <param name="courseAttachedTo">The course the activity belongs to</param>
        internal Activity(List<DataContext.Activity> oldActivitiesOfSameType, string activityType, Course courseAttachedTo)
        {
            Course = courseAttachedTo;
            ActivityType = activityType;
            Sections = new List<Section>();

            foreach (DataContext.Activity oldActivity in oldActivitiesOfSameType)
                Sections.Add(new Section(oldActivity, this));
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