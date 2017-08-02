using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.DataModels
{
    public class Building
    {
        // Constructor used to convert the building in datacontext to the building in datamodels
        internal Building(DataContext.Building oldBuilding)
        {
            BuildingName = oldBuilding.BuildingName;
            BuildingCode = oldBuilding.BuildingCode;
            Address = oldBuilding.Address;
            Latitude = oldBuilding.Latitude.GetValueOrDefault(0);
            Longitude = oldBuilding.Longitude.GetValueOrDefault(0);
        }

        public string BuildingName { get; set; }
        public string BuildingCode { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
