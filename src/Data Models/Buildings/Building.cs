using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.DataModels
{
    /// <summary>
    /// A class used to hold information about a building
    /// </summary>
    public class Building
    {
        /// <summary>
        /// Constructor used to convert the building in datacontext to the building in datamodels
        /// </summary>
        /// <param name="oldBuilding">Raw data from the database</param>
        internal Building(DataContext.Building oldBuilding)
        {
            BuildingName = oldBuilding.BuildingName;
            BuildingCode = oldBuilding.BuildingCode;
            Address = oldBuilding.Address;
            Latitude = oldBuilding.Latitude.GetValueOrDefault(0);
            Longitude = oldBuilding.Longitude.GetValueOrDefault(0);
        }

        /// <summary>
        /// Get / set the building name
        /// </summary>
        public string BuildingName { get; set; }

        /// <summary>
        /// Get / set the building code
        /// </summary>
        public string BuildingCode { get; set; }

        /// <summary>
        /// Get / set the address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Get / set the latitude
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Get / set the longitude
        /// </summary>
        public double Longitude { get; set; }
    }
}
