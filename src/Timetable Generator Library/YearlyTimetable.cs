using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    /// <summary>
    /// A class containing two timetables for one year
    /// </summary>
    public class YearlyTimetable
    {
        private SeasonalTimetable fallTimetable = new SeasonalTimetable();
        private SeasonalTimetable winterTimetable = new SeasonalTimetable();

        /// <summary>
        /// Get all the fall sections
        /// </summary>
        /// <returns>Fall sections</returns>
        public List<Section> GetFallSections()
        {
            return fallTimetable.Sections.ToList();
        }

        /// <summary>
        /// Get all the winter sections
        /// </summary>
        /// <returns>Winter sections</returns>
        public List<Section> GetWinterSections()
        {
            return winterTimetable.Sections.ToList();
        }

        /// <summary>
        /// Determines if a section can be placed in this timetable
        /// Pre-condition: term must be 'F', 'W', or 'Y'
        /// </summary>
        /// <param name="section">The section to add</param>
        /// <param name="term">The term, meeting the pre-conditions</param>
        /// <returns>True if the section fits in this timetable; else false</returns>
        public bool DoesSectionFit(Section section, char term)
        {
            switch (term)
            {
                case 'F':
                    return fallTimetable.DoesSectionFit(section);
                case 'S':
                    return winterTimetable.DoesSectionFit(section);
                case 'Y':
                    return winterTimetable.DoesSectionFit(section) && fallTimetable.DoesSectionFit(section);
                default:
                    throw new NotImplementedException(term + " was not handled before! ");
            }
        }

        /// <summary>
        /// Adds a section to this timetable if it fits to this timetable
        /// Pre-condition: term must be 'F', 'S', or 'Y'
        /// </summary>
        /// <param name="section">The section to add</param>
        /// <param name="term">The term, meeting the pre-conditions</param>
        /// <returns>True if the section is added; else false</returns>
        public bool AddSection(Section section, char term)
        {
            switch (term)
            {
                case 'F':
                    return fallTimetable.AddSection(section);
                case 'S':
                    return winterTimetable.AddSection(section);
                case 'Y':
                    return winterTimetable.AddSection(section) && fallTimetable.AddSection(section);
                default:
                    throw new NotImplementedException(term + " was not handled before! ");
            }
        }

        /// <summary>
        /// Get the total time spent in between classes over the two terms
        /// </summary>
        public double TotalTimeBetweenClasses
        {
            get { return fallTimetable.TotalTimeBetweenClasses + winterTimetable.TotalTimeBetweenClasses; }
        }

        /// <summary>
        /// Get the start time (24-hr time) of the earliest class from the two terms
        /// </summary>
        public double EarliestClassTime
        {
            get { return Math.Min(fallTimetable.EarliestClasstime, winterTimetable.EarliestClasstime); }
        }

        /// <summary>
        /// Get the end time (24-hr time) of the latest class from the two terms
        /// </summary>
        public double LatestClassTime
        {
            get { return Math.Max(fallTimetable.LatestClasstime, winterTimetable.LatestClasstime); }
        }

        /// <summary>
        /// Get the total time spent in class over the two terms
        /// </summary>
        public double TimeInClass
        {
            get { return fallTimetable.TimeInClass + winterTimetable.TimeInClass; }
        }

        /// <summary>
        /// Get the total time spent walking in between classes over the two terms
        /// </summary>
        public double TotalWalkDuration
        {
            get { return fallTimetable.TotalWalkDuration + winterTimetable.TotalWalkDuration; }
        }

        /// <summary>
        /// Get the walk duration in between back-to-back classes over the two terms
        /// </summary>
        public List<double> WalkDurationInBackToBackClasses
        {
            get
            {
                List<double> walkDurations = new List<double>();
                walkDurations.AddRange(fallTimetable.WalkDurationInBackToBackClasses);
                walkDurations.AddRange(winterTimetable.WalkDurationInBackToBackClasses);
                return walkDurations;
            }
        }
    }
}
