using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.Generator
{
    /// <summary>
    /// A class used to store the restrictions for a timetable generator
    /// </summary>
    public class Restrictions
    {
        /// <summary>
        /// Get / set the time for the earliest class in two terms (24-hr time)
        /// </summary>
        public double? EarliestClass { get; set; }

        /// <summary>
        /// Get / set the time for the latest class in the two terms (24-hr time)
        /// </summary>
        public double? LatestClass { get; set; }

        /// <summary>
        /// Get / set the walk duration in between back to back classes in the two terms
        /// (minutes)
        /// </summary>
        public double? WalkDurationInBackToBackClasses { get; set; }
    }
}