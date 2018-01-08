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
    public class YearlyTimetable : IUniversityTimetable
    {
        private SeasonalTimetable fallTimetable = new SeasonalTimetable();
        private SeasonalTimetable winterTimetable = new SeasonalTimetable();

        /// <summary>
        /// Get the average walking distance
        /// </summary>
        public double AverageWalkingDistance
        {
            get { throw new NotImplementedException(); }
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

        /// <summary>
        /// Get the end time (24-hr time) of the latest class from the two terms
        /// </summary>
        public double LatestClassTime
        {
            get { return Math.Max(fallTimetable.LatestClassTime, winterTimetable.LatestClassTime); }
        }

        /// <summary>
        /// Get the start time (24-hr time) of the earliest class from the two terms
        /// </summary>
        public double EarliestClassTime
        {
            get { return Math.Min(fallTimetable.EarliestClassTime, winterTimetable.EarliestClassTime); }
        }

        /// <summary>
        /// Get the total time spent in between classes over the two terms
        /// </summary>
        public double TotalTimeBetweenClasses
        {
            get { return fallTimetable.TotalTimeBetweenClasses + winterTimetable.TotalTimeBetweenClasses; }
        }

        /// <summary>
        /// Get the total time spent in class over the two terms
        /// </summary>
        public double TimeInClass
        {
            get { return fallTimetable.TimeInClass + winterTimetable.TimeInClass; }
        }

        /// <summary>
        /// Get the total number of days in class
        /// </summary>
        public int NumDaysInClass
        {
            get { return Math.Max(fallTimetable.NumDaysInClass, winterTimetable.NumDaysInClass); }
        }

        /// <summary>
        /// Get a list of weekdays that have classes on that day
        /// </summary>
        public HashSet<int> DaysInClass
        {
            get
            {
                HashSet<int> totalDays = fallTimetable.DaysInClass;
                totalDays.UnionWith(winterTimetable.DaysInClass);
                return totalDays;
            }
        }

        /// <summary>
        /// Return a list of time duration between classes
        /// </summary>
        public List<double> TimeBetweenClasses
        {
            get
            {
                List<double> combinedTime = new List<double>();
                combinedTime.AddRange(fallTimetable.TimeBetweenClasses);
                combinedTime.AddRange(winterTimetable.TimeBetweenClasses);
                return combinedTime;
            }
        }

        /// <summary>
        /// Adds a section to this timetable if it fits to this timetable
        /// </summary>
        /// <param name="section">The section to add</param>
        /// <returns>True if the section is added; else false</returns>
        public bool AddSection(Section section)
        {
            return AddSection(section, section.Activity.Course.Term.ToString());
        }

        public bool AddSection(Section section, string term)
        {
            switch (term)
            {
                case "F":
                    return fallTimetable.AddSection(section);
                case "S":
                    return winterTimetable.AddSection(section);
                case "Y":
                    return winterTimetable.AddSection(section) && fallTimetable.AddSection(section);
                default:
                    throw new NotImplementedException(section.Activity.Course.Term + " was not handled before! ");
            }
        }

        /// <summary>
        /// Determines whether a section exists in this timetable
        /// </summary>
        /// <param name="section">A section</param>
        /// <returns>True if it exists in this timetable, else false</returns>
        public bool Contains(Section section)
        {
            return fallTimetable.Contains(section) || winterTimetable.Contains(section);
        }

        /// <summary>
        /// Deletes a section from this timetable
        /// </summary>
        /// <param name="section">The section to delete</param>
        /// <returns>True if it was found and deleted successfully; else false</returns>
        public bool DeleteSection(Section section)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines if a section can be placed in this timetable
        /// </summary>
        /// <param name="section">The section to add</param>
        /// <returns>True if the section fits in this timetable; else false</returns>
        public bool DoesSectionFit(Section section)
        {
            switch (section.Activity.Course.Term)
            {
                case "F":
                    return fallTimetable.DoesSectionFit(section);
                case "S":
                    return winterTimetable.DoesSectionFit(section);
                case "Y":
                    return winterTimetable.DoesSectionFit(section) && fallTimetable.DoesSectionFit(section);
                default:
                    throw new NotImplementedException(section.Activity.Course.Term + " was not handled before! ");
            }
        }

        /// <summary>
        /// Get all the fall sections
        /// </summary>
        /// <returns>Fall sections</returns>
        public List<Section> GetFallSections()
        {
            return fallTimetable.GetSections().ToList();
        }

        /// <summary>
        /// Get all the winter sections
        /// </summary>
        /// <returns>Winter sections</returns>
        public List<Section> GetWinterSections()
        {
            return winterTimetable.GetSections().ToList();
        }

        /// <summary>
        /// Get a list of sections that exist in this timetable
        /// </summary>
        /// <returns></returns>
        public List<Section> GetSections()
        {
            List<Section> combinedSections = new List<Section>();
            combinedSections.AddRange(GetFallSections());
            combinedSections.AddRange(GetWinterSections());
            return combinedSections;
        }

        /// <summary>
        /// Make a hardcopy of this timetable
        /// </summary>
        /// <returns>A hard copy of this timetable</returns>
        public ITimetable MakeCopy()
        {
            YearlyTimetable copy = new YearlyTimetable();
            copy.fallTimetable = (SeasonalTimetable) fallTimetable.MakeCopy();
            copy.winterTimetable = (SeasonalTimetable) winterTimetable.MakeCopy();
            return copy;
        }

        public void Show()
        {
            fallTimetable.Show();
            winterTimetable.Show();
        }
    }
}
