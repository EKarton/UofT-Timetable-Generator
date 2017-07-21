using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.DataModels
{
    public class Time
    {
        public string Day { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public string FallLocation { get; set; }
        public string WinterLocation { get; set; }
    }
}
