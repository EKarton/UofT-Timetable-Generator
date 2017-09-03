using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;
using UoftTimetableGenerator.Generator;

namespace UoftTimetableGenerator.WebAPI.Models
{
    /// <summary>
    /// A class used to represent a year's timetable (Fall and Winter timetables)
    /// </summary>
    internal class SimplifiedYearlyTimetable
    {
        /// <summary>
        /// Constructs a SimplifiedYearlyTimetable
        /// </summary>
        /// <param name="existingTimetable">The existing yearly timetable</param>
        /// <param name="name">The name of this timetable</param>
        public SimplifiedYearlyTimetable(YearlyTimetable existingTimetable, string name)
        {
            Name = name;
            TotalTimeBetweenClasses = existingTimetable.TotalTimeBetweenClasses;
            TotalTimeInClass = existingTimetable.TimeInClass;

            // Populating fall-timetable blocks
            foreach (Section section in existingTimetable.GetFallSections())
                foreach (Session session in section.Sessions)
                {
                    char term = session.Section.Activity.Course.Term[0];
                    SimplifiedTimetableBlock block = new SimplifiedTimetableBlock(session, term);
                    FallTimetableBlocks.Add(block);
                }

            // Getting winter-timetable blocks
            foreach (Section section in existingTimetable.GetWinterSections())
                foreach (Session session in section.Sessions)
                {
                    char term = session.Section.Activity.Course.Term[0];
                    SimplifiedTimetableBlock block = new SimplifiedTimetableBlock(session, term);
                    WinterTimetableBlocks.Add(block);
                }
        }

        /// <summary>
        /// Get / set the name of this timetable
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get / set the timetable blocks in the fall term
        /// </summary>
        public List<SimplifiedTimetableBlock> FallTimetableBlocks = new List<SimplifiedTimetableBlock>();

        /// <summary>
        /// Get / set the timetable blocks in the winter term
        /// </summary>
        public List<SimplifiedTimetableBlock> WinterTimetableBlocks = new List<SimplifiedTimetableBlock>();

        /// <summary>
        /// Get / set the total walking duration in both the fall and winter terms
        /// </summary>
        public double TotalWalkingDuration { get; set; }

        /// <summary>
        /// Get / set the total amount of time spent in class in both the fall and winter terms
        /// </summary>
        public double TotalTimeInClass { get; set; }

        /// <summary>
        /// Get / set the total amount of time spent between classes in both the fall and winter terms
        /// </summary>
        public double TotalTimeBetweenClasses { get; set; }
    }
}
