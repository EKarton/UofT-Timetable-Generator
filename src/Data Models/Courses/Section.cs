using System.Collections.Generic;

namespace UoftTimetableGenerator.DataModels
{
    public class Section
    {
        public Section()
        {
            Activity = null;
            SectionCode = "";
            Instructors = new List<string>();
            Sessions = new List<Session>();
        }

        public Section(DataContext.Activity oldActivity, Activity activityAttachedTo)
        {
            Activity = activityAttachedTo;
            SectionCode = oldActivity.ActivityCode;
            Instructors = new List<string>();
            Sessions = new List<Session>();
            foreach (DataContext.Session oldSession in oldActivity.Sessions)
                Sessions.Add(new Session(oldSession, this));
        }

        public Activity Activity { get; set; }
        public string SectionCode { get; set; }
        public List<string> Instructors { get; set; }
        public List<Session> Sessions { get; set; }
    }
}