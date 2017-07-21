using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.DataModels
{
    public class Course
    {
        public string CourseName { get; set; }
        public string Term { get; set; }
        public MeetingSession[] Lectures { get; set; }
        public MeetingSession[] Tutorials { get; set; }
        public MeetingSession[] Practicals { get; set; }
    }
}
