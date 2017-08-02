using System.Collections.Generic;

namespace UoftTimetableGenerator.DataModels
{
    public class Section
    {
        public Section()
        {
        }

        public Section(DataContext.Activity oldActivity)
        {
            SectionCode = oldActivity.ActivityCode;
            Sessions = new List<Session>();
            foreach (DataContext.Session oldSession in oldActivity.Sessions)
                Sessions.Add(new Session(oldSession));
        }

        public string SectionCode { get; set; }
        public List<string> Instructors { get; set; }
        public List<Session> Sessions { get; set; }
    }
}