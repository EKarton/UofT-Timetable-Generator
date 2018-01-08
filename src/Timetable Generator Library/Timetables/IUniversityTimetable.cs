using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    public interface IUniversityTimetable : ITimetable
    {
        double AverageWalkingDistance { get; }
        double TotalWalkDuration { get; }
        List<double> WalkDurationInBackToBackClasses { get; }

        double LatestClassTime { get; }
        double EarliestClassTime { get; }
        double TotalTimeBetweenClasses { get; }
        double TimeInClass { get; }
        int NumDaysInClass { get; }
        HashSet<int> DaysInClass { get; }
        List<double> TimeBetweenClasses { get; }
    }
}
