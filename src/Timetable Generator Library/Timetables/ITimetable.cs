using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    public interface ITimetable
    {
        double AverageWalkingDistance { get; }
        double TotalWalkDuration { get; }
        List<double> WalkDurationInBackToBackClasses { get; }

        double LatestClassTime { get; }
        double EarliestClassTime { get; }
        double TotalTimeBetweenClasses { get; }
        double TimeInClass { get; }
        int NumDaysInClass { get; }
        List<string> DaysInClass { get; }
        List<double> TimeBetweenClasses { get; }

        bool AddSection(Section section);
        bool Contains(Section section);
        bool DeleteSection(Section section);
        bool DoesSectionFit(Section section);
        List<Section> GetSections();
        ITimetable MakeCopy();
    }
}
