using System.Collections.Generic;

namespace UoftTimetableGenerator.DataModels
{
    public class Activity
    {
        // A constructor used to convert the DataContext.Activity to Datamodels.Activity
        internal Activity(List<DataContext.Activity> oldActivitiesOfSameType, string activityType, Course courseAttachedTo)
        {
            Course = courseAttachedTo;
            ActivityType = activityType;
            Sections = new List<Section>();

            foreach (DataContext.Activity oldActivity in oldActivitiesOfSameType)
                Sections.Add(new Section(oldActivity, this));
        }

        public Course Course { get; set; }
        public string ActivityType { get; set; }
        public List<Section> Sections { get; set; }
    }
}