using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    public class YearlyTimetable
    {
        private SeasonalTimetable fallTimetable = new SeasonalTimetable();
        private SeasonalTimetable winterTimetable = new SeasonalTimetable();

        public List<Section> GetFallSections()
        {
            return fallTimetable.Sections.ToList();
        }

        public List<Section> GetWinterSections()
        {
            return winterTimetable.Sections.ToList();
        }

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

        public double TotalTimeBetweenClasses
        {
            get { return fallTimetable.TotalTimeBetweenClasses + winterTimetable.TotalTimeBetweenClasses; }
        }

        public double EarliestClassTime
        {
            get { return Math.Min(fallTimetable.EarliestClasstime, winterTimetable.EarliestClasstime); }
        }

        public double LatestClassTime
        {
            get { return Math.Max(fallTimetable.LatestClasstime, winterTimetable.LatestClasstime); }
        }

        public double TimeInClass
        {
            get { return fallTimetable.TimeInClass + winterTimetable.TimeInClass; }
        }

        public double TotalWalkDuration
        {
            get { return fallTimetable.TotalWalkDuration + winterTimetable.TotalWalkDuration; }
        }

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
