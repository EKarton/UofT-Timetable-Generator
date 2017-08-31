using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.Generator
{
    public class Preferences
    {
        public enum Day
        {
            Undefined,
            Morning,
            Afternoon,
            Evening,
            Night
        }

        public enum Quantity
        {
            Undefined,
            Minimum,
            Maximum
        }

        public Day ClassType { get; set; }
        public Quantity WalkingDistance { get; set; }
        public Quantity NumDaysInClass { get; set; }
        public Quantity TimeBetweenClasses { get; set; }
        public double? LunchPeriod { get; set; }
    }
}