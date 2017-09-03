using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;
using UoftTimetableGenerator.Generator;

namespace UoftTimetableGenerator.WebAPI.Models
{
    /// <summary>
    /// A class used to store which timetables to generate
    /// </summary>
    public class TimetableRequest
    {
        /// <summary>
        /// Get / set the course codes for which the timetables generated will contain
        /// </summary>
        public string[] CourseCodes { get; set; }

        /// <summary>
        /// Get / set the preferences that all generated timetables will try to achieve
        /// </summary>
        public Preferences Preferences { get; set; }

        /// <summary>
        /// Get / set the restrictions that all generated timetables will have to achieve
        /// </summary>
        public Restrictions Restrictions { get; set; }
    }
}
