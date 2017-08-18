using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.WebAPI.Models
{
    internal class SimplifiedTimetableBlock
    {
        public SimplifiedTimetableBlock(Session session, char term)
        {
            CourseCode = session.Section.Activity.Course.CourseCode;
            ActivityType = session.Section.Activity.ActivityType;

            SectionCode = session.Section.SectionCode;
            Instructors = session.Section.Instructors.ToArray();

            StartTime = session.StartTime % 100;
            EndTime = session.EndTime % 100;
            StartDay = (int) session.StartTime / 100;
            EndDay = (int) session.EndTime / 100;

            switch(term)
            {
                case 'F':
                    BuildingCode = session.FallBuildingCode;
                    RoomNumber = session.FallRoomNumber;
                    break;
                case 'S':
                    BuildingCode = session.WinterBuildingCode;
                    RoomNumber = session.WinterRoomNumber;
                    break;
                case 'Y':
                    break;
                default:
                    throw new Exception(term + " - this term is not handled before!");
            }
        }

        public string CourseCode { get; set; }
        public string ActivityType { get; set; }
        public string SectionCode { get; set; }
        public string[] Instructors { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public int StartDay { get; set; }
        public int EndDay { get; set; }
        public string BuildingCode { get; set; }
        public string RoomNumber { get; set; }
    }
}
