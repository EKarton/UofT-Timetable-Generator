﻿using System.Collections.Generic;

namespace UoftTimetableGenerator.DataModels
{
    /// <summary>
    /// A class representing a section of a course
    /// </summary>
    public class Section
    {
        /// <summary>
        /// Creates an empty section
        /// </summary>
        public Section()
        {
            Activity = null;
            SectionCode = "";
            Instructors = new List<string>();
            Sessions = new List<Session>();
        }

        /// <summary>
        /// Get / set the activity the section belongs to
        /// </summary>
        public Activity Activity { get; set; }

        /// <summary>
        /// Get / set the section code of this section
        /// </summary>
        public string SectionCode { get; set; }

        /// <summary>
        /// Get / set a list of instructors that will be teaching in this section
        /// </summary>
        public List<string> Instructors { get; set; }

        /// <summary>
        /// Get / set a list of sessions this section is comprised of
        /// </summary>
        public List<Session> Sessions { get; set; }

        /// <summary>
        /// Return the earliest session's start time
        /// </summary>
        public double MaxStartTime
        {
            get
            {
                double minStartTime = 1000;
                foreach (Session session in Sessions)
                {
                    if (session.StartTime< minStartTime)
                        minStartTime = session.StartTime;
                }
                return minStartTime;
            }
        }

        /// <summary>
        /// Return the latest session's start time
        /// </summary>
        public double MaxEndTime
        {
            get
            {
                double maxEndTime = 0;
                foreach (Session session in Sessions)
                {
                    if (session.EndTime> maxEndTime)
                        maxEndTime = session.EndTime;
                }
                return maxEndTime;
            }
        }
    }
}