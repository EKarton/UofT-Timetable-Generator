using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.DataModels
{
    public class MeetingSession
    {
        private static int numMade = 0;
        private int id = 0;

        public MeetingSession()
        {
            id = numMade;
            numMade++;
        }

        public int ID { get; set; }
        public string MeetingCode { get; set; }
        public string Instructor { get; set; }
        public Time[] Times { get; set; }
    }
}
