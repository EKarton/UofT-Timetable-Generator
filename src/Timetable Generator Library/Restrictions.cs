using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.Generator
{
    public class Restrictions
    {
        public double? EarliestClass { get; set; }
        public double? LatestClass { get; set; }
        public double? WalkDurationInBackToBackClasses { get; set; }
    }
}