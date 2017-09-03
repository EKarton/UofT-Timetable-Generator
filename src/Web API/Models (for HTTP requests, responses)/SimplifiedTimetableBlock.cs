using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.WebAPI.Models
{
    /// <summary>
    /// A class that represents a session for a timetable in a webpage
    /// </summary>
    internal class SimplifiedTimetableBlock
    {
        /// <summary>
        /// Constructs a SimplifiedTimetableBlock
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="term">The term ('F', 'S', or 'Y')</param>
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

        /// <summary>
        /// Get / set the course code of this timetable session
        /// </summary>
        public string CourseCode { get; set; }

        /// <summary>
        /// Get / set the activity type of this timetable session
        /// </summary>
        public string ActivityType { get; set; }

        /// <summary>
        /// Get / set the section code of this timetable session
        /// </summary>
        public string SectionCode { get; set; }

        /// <summary>
        /// Get / set the instructors that will teach in this timetable session
        /// </summary>
        public string[] Instructors { get; set; }

        /// <summary>
        /// Get / set the 24-hr time which this session starts at
        /// </summary>
        public double StartTime { get; set; }

        /// <summary>
        /// Get / set the 24-hr time which this session starts at
        /// </summary>
        public double EndTime { get; set; }

        /// <summary>
        /// Get / set the weekday index which this session starts at
        /// </summary>
        public int StartDay { get; set; }

        /// <summary>
        /// Get / set the weekday index which this session starts at
        /// </summary>
        public int EndDay { get; set; }

        /// <summary>
        /// Get / set the building code which the session will take place at
        /// </summary>
        public string BuildingCode { get; set; }

        /// <summary>
        /// Get / set the room number which the session will take place at
        /// </summary>
        public string RoomNumber { get; set; }
    }
}
