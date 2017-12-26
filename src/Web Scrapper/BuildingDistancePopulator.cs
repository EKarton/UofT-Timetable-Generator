using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.WebScrapper
{
    /// <summary>
    /// A class used to calculate and store the distances and time in between buildings into the database
    /// </summary>
    class BuildingDistancePopulator
    {
        /// <summary>
        /// Updates the distances between any two buildings in the database
        /// </summary>
        /// <param name="db">The database context</param>
        /// <param name="existingDistances">The existing distances</param>
        private void UpdateBuildingDistances(UofTDataContext db, BuildingDistance existingDistances)
        {
            Building building1 = existingDistances.Building;
            Building building2 = existingDistances.Building1;

            Console.WriteLine("Attempting to update data " + building1.BuildingCode + " -> " + building2.BuildingCode);

            // Getting distance info (like walk durations, etc)
            BuildingDistanceCalculator calculator = new BuildingDistanceCalculator();
            DataModels.BuildingDistance buildingDistance = calculator.GetInfoBetweenBuildings("walking", building1.Address, building2.Address);

            // Making changes to the data
            existingDistances.WalkingDistance = buildingDistance.WalkingDistance;
            existingDistances.WalkingDuration = buildingDistance.WalkDuration;
            existingDistances.CyclingDistance = buildingDistance.CyclingDistance;
            existingDistances.CyclingDuration = buildingDistance.CyclingDuration;
            existingDistances.TransitDistance = buildingDistance.PublicTransitDistance;
            existingDistances.TransitDuration = buildingDistance.PublicTransitDuration;
            existingDistances.DrivingDistance = buildingDistance.DrivingDistance;
            existingDistances.DrivingDuration = buildingDistance.DrivingDuration;

            db.SubmitChanges();
            Console.WriteLine("Successful! Has updated data for " + building1.BuildingCode + " -> " + building2.BuildingCode);
        }

        /// <summary>
        /// Adds the distance for two buildings in the database
        /// </summary>
        /// <param name="db">The database context</param>
        /// <param name="building1">The first building</param>
        /// <param name="building2">The second building</param>
        private void AddBuildingDistances(UofTDataContext db, Building building1, Building building2)
        {
            Console.WriteLine("Getting data " + building1.Address + " -> " + building2.Address);

            // Getting distance info (like walk durations, etc)
            BuildingDistanceCalculator calculator = new BuildingDistanceCalculator();
            DataModels.BuildingDistance buildingDistance = calculator.GetInfoBetweenBuildings("walking", building1.Address, building2.Address);

            BuildingDistance newBuildingDistance = new BuildingDistance()
            {
                Building = building1,
                Building1 = building2,
                WalkingDistance = buildingDistance.WalkingDistance,
                WalkingDuration = buildingDistance.WalkDuration,
                CyclingDistance = buildingDistance.CyclingDistance,
                CyclingDuration = buildingDistance.CyclingDuration,
                TransitDistance = buildingDistance.PublicTransitDistance,
                TransitDuration = buildingDistance.PublicTransitDuration,
                DrivingDistance = buildingDistance.DrivingDistance,
                DrivingDuration = buildingDistance.DrivingDuration
            };

            Console.WriteLine("Inserting new data " + building1.BuildingCode + " -> " + building2.BuildingCode);
            db.BuildingDistances.InsertOnSubmit(newBuildingDistance);
            db.SubmitChanges();
        }

        /// <summary>
        /// Updates all the distances already stored in the database
        /// </summary>
        public void UpdateBuildingDistanceInfo()
        {
            using (UofTDataContext db = new UofTDataContext())
            {
                List<BuildingDistance> rows = db.BuildingDistances.ToList();
                foreach (BuildingDistance row in rows)
                {
                    UpdateBuildingDistances(db, row);
                }
            }
        }

        /// <summary>
        /// Insert new building distances into the database for every new building that exists in the Buildings table.
        /// If {skipExistingData} param is true:
        ///     - it will skip the pair of buildings whose distances already exists in the database.
        ///     - it will add the distances for any pair of buildings whose distances do not exist in the database
        /// 
        /// If {skipExistingData} param is false:
        ///     - it will update the distances already existing in the database
        ///     - it will add the distances for any pair of buildings whose distances do not exist in the database
        /// </summary>
        /// <param name="skipExistingData">Whether to skip any existing data or not</param>
        public void InsertBuildingDistancesToDatabase(bool skipExistingData)
        {
            using (UofTDataContext db = new UofTDataContext())
            {
                List<Building> buildings = db.Buildings.ToList();
                for (int i = 0; i < buildings.Count; i++)
                    for (int j = i + 1; j < buildings.Count; j++)
                    {
                        Building building1 = buildings[i];
                        Building building2 = buildings[j];
                        Console.WriteLine("Attempting to insert data " + building1.BuildingCode + " -> " + building2.BuildingCode);

                        // Getting pre-existing data for these two buildings
                        var existingData = from row in db.BuildingDistances
                                           where (row.BuildingID1 == building1.BuildingID && row.BuildingID2 == building2.BuildingID) ||
                                                       (row.BuildingID1 == building2.BuildingID && row.BuildingID2 == building1.BuildingID)
                                           select row;

                        if (existingData != null && existingData.ToList().Count == 1 && !skipExistingData)
                        {
                            Console.WriteLine("Updating building distances " + building1.BuildingCode + " -> " + building2.BuildingCode); 
                            foreach (BuildingDistance row in existingData.ToList())
                                UpdateBuildingDistances(db, row);
                        }
                        else if (existingData == null || existingData.ToList().Count == 0)
                        {
                            AddBuildingDistances(db, building1, building2);
                        }
                    }
            }
        }

        /// <summary>
        /// Deletes all the records in the BuildingDistance table
        /// </summary>
        public void DeleteAllRecords()
        {
            using (UofTDataContext db = new UofTDataContext())
            {
                db.ExecuteCommand("DELETE FROM BuildingDistances");
                db.ExecuteCommand("DBCC CHECKIDENT ('BuildingDistances', RESEED, 0)");
            }
        }
    }
}
