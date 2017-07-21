using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.DataModels
{
    public class TimetableSession
    {
        private string courseName = "";
        private MeetingSession session = null;

        public TimetableSession(MeetingSession session, Course courseOfSession)
        {
            courseName = courseOfSession.CourseName;
            this.session = session;
        }

        public string Coursecode
        {
            get { return courseName; }
        }

        public MeetingSession Session
        {
            get { return session; }
        }
    }
}
