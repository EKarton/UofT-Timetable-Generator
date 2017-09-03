using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.Generator
{
    /// <summary>
    /// An interface to be implemented in all timetable generator algorithms
    /// </summary>
    public interface ITimetableGenerator
    {
        /// <summary>
        /// Generates the timetables
        /// </summary>
        /// <returns>A list of generated timetables</returns>
        List<YearlyTimetable> GetTimetables();
    }
}
