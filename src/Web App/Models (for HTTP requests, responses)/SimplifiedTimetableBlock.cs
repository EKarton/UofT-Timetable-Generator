﻿using System;
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

            SectionCode = session.Section.SectionCode;
            Instructors = session.Section.Instructors.ToArray();

            StartTime = session.StartTimeWithWeekday % 100;
            EndTime = session.EndTimeWithWeekday % 100;
            StartDay = (int) session.StartTimeWithWeekday / 100;
            EndDay = (int) session.EndTimeWithWeekday / 100;

            // Convert the abreviated activity type to its full type.
            switch (session.Section.Activity.ActivityType)
            {
                case "L":
                    ActivityType = "Lecture";
                    break;
                case "P":
                    ActivityType = "Practical";
                    break;
                case "T":
                    ActivityType = "Tutorial";
                    break;
                default:
                    ActivityType = session.Section.Activity.ActivityType;
                    break;
            }
            
            // Handle the locations
            switch(term)
            {
                case 'F':
                    if (session.FallBuilding != null)
                    {
                        BuildingCode = session.FallBuilding.BuildingCode;
                        RoomNumber = session.FallRoomNumber;
                    }
                    break;
                case 'S':
                    if (session.WinterBuilding != null)
                    {
                        BuildingCode = session.WinterBuilding.BuildingCode;
                        RoomNumber = session.WinterRoomNumber;
                    }
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
