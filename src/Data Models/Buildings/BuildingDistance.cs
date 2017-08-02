using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.DataModels
{
    public class BuildingDistance
    {
        public Building Building1 { get; set; }
        public Building Building2 { get; set; }
        public double WalkDuration { get; set; }  // In minutes
        public double WalkingDistance { get; set; }  // In kilometers
        public double CyclingDuration { get; set; }  // In minutes
        public double CyclingDistance { get; set; }  // In kilometers
        public double PublicTransitDuration { get; set; }  // In minutes
        public double PublicTransitDistance { get; set; }  // In kilometers
        public double DrivingDuration { get; set; } // In minutes
        public double DrivingDistance { get; set; } // In kilometers
    }
}
