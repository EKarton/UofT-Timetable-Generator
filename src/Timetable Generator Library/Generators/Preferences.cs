using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.Generator
{
    /// <summary>
    /// A class used to hold the preference settings for the timetable generators
    /// </summary>
    public class Preferences
    {
        /// <summary>
        /// Time of day
        /// </summary>
        public enum Day
        {
            Undefined,
            Morning,
            Afternoon,
            Evening,
            Night
        }

        /// <summary>
        /// Quantity
        /// </summary>
        public enum Quantity
        {
            Undefined,
            Minimum,
            Maximum
        }

        /// <summary>
        /// Get / set the class type for the two terms
        /// </summary>
        public Day ClassType { get; set; }

        /// <summary>
        /// Get / set the total walking distance for the two terms
        /// </summary>
        public Quantity WalkingDistance { get; set; }

        /// <summary>
        /// Get / set the number of days in class for the two terms
        /// </summary>
        public Quantity NumDaysInClass { get; set; }

        /// <summary>
        /// Get / set the time between classes for the two terms
        /// </summary>
        public Quantity TimeBetweenClasses { get; set; }

        /// <summary>
        /// Get / set the amount of time spent for lunch (in minutes)
        /// </summary>
        public double? LunchPeriod { get; set; }

        public bool AvoidLongSessions { get; set; }
    }
}