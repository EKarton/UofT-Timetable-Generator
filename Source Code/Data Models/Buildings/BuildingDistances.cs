using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.DataModels
{
    public class BuildingDistances
    {
        public string[] Buildings { get; set; }

        public TimeSpan WalkingTime { get; set; }

        public Distance WalkingDistance { get; set; }

        public TimeSpan CyclingTime { get; set; }

        public Distance CyclingDistance { get; set; }

        public TimeSpan DrivingTime { get; set; }

        public Distance DrivingDistance { get; set; }
    }
}
