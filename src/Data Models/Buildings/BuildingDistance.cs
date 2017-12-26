using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.DataModels
{
    /// <summary>
    /// A class used to hold distance/duration information between two buildings
    /// </summary>
    public class BuildingDistance
    {
        /// <summary>
        /// Get / set the walking duration in between the two buildings (in minutes)
        /// </summary>
        public double? WalkDuration { get; set; }

        /// <summary>
        /// Get / set the walking distance in between the two buildings (in km)
        /// </summary>
        public double? WalkingDistance { get; set; }

        /// <summary>
        /// Get / set the cycling duration in between the two buildings (in minutes)
        /// </summary>
        public double? CyclingDuration { get; set; }

        /// <summary>
        /// Get / set the cycling distance in between the two buildings (in km)
        /// </summary>
        public double? CyclingDistance { get; set; } 

        /// <summary>
        /// Get / set the traveling time it takes to get from the first building 
        /// to the second using public transit (in minutes)
        /// </summary>
        public double? PublicTransitDuration { get; set; } 

        /// <summary>
        /// Get / set the distance travelled by public transit in between two buildings (in km)
        /// </summary>
        public double? PublicTransitDistance { get; set; } 

        /// <summary>
        /// Get / set the driving duration in between the two buildings (in min)
        /// </summary>
        public double? DrivingDuration { get; set; }

        /// <summary>
        /// Get / set the driving distance in between two buildings (in km)
        /// </summary>
        public double? DrivingDistance { get; set; }
    }
}
