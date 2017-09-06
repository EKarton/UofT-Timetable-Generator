using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace UoftTimetableGenerator.WebScrapper
{
    /// <summary>
    /// A class used to calculate and store the distances and time in between buildings into the database
    /// </summary>
    class BuildingDistancePopulator
    {
        private const string GOOGLEAPI_KEY = "AIzaSyAna8lO72_VyFrLYjYMTvVgPcvmlU_fCrc"; //"AIzaSyCVCnQ3iBa6S4W8gZkTbwO88d7qhKaGIC8";

        /// <summary>
        /// Parses the time from the google maps API to a C# Timespan object
        /// </summary>
        /// <param name="time">The time, from the google maps API</param>
        /// <returns>The Timespan object representation</returns>
        private TimeSpan ParseTimespan(string time)
        {
            int days = 0;
            int hours = 0;
            int minutes = 0;
            int seconds = 0;
            int milliseconds = 0;

            string[] times = time.Split(' ');
            for (int i = 0; i < times.Length; i += 2)
            {
                int amount = Convert.ToInt32(times[i]);
                string type = times[i + 1].ToLower();

                if (type == "day" || type == "days")
                    days = amount;
                else if (type == "hour" || type == "hours")
                    hours = amount;
                else if (type == "min" || type == "mins")
                    minutes = amount;
                else if (type == "sec" || type == "secs")
                    seconds = amount;
            }

            return new TimeSpan(days, hours, minutes, seconds, milliseconds);
        }

        /// <summary>
        /// Parses the distance that came from the google maps API into a double.
        /// </summary>
        /// <param name="distance">The distance, from the google maps API</param>
        /// <returns>The distance, in kilometers</returns>
        private double ParseDistance(string distance)
        {
            double kilometers = 0;
            double meters = 0;
            double centimeters = 0;
            double millimeters = 0;

            string[] distances = distance.Split(' ');
            for (int i = 0; i < distances.Length; i += 2)
            {
                double amount = Convert.ToDouble(distances[i]);
                string type = distances[i + 1].ToLower();

                if (type == "km")
                    kilometers = amount;
                else if (type == "m")
                    meters = amount;
                else if (type == "cm")
                    centimeters = amount;
                else if (type == "mm")
                    millimeters = amount;
            }

            return kilometers + (meters / 1000) + (centimeters / 100000) + (millimeters / 1000000);
        }

        /// <summary>
        /// Get the distance and time spent when traveling between two buildings
        /// Pre-condition: {mode} must be a supported mode of transportation from the google maps api.
        /// Please refer to the google maps api for more details.
        /// </summary>
        /// <param name="mode">The mode of transportation</param>
        /// <param name="address1">The address of the first building</param>
        /// <param name="address2">The address of the second building</param>
        /// <returns>A dictionary, where dict["Distance"] is the distance in km, and dict["Duration"] is the time spent</returns>
        private Dictionary<string, double?> GetInfoBetweenBuildings(string mode, string address1, string address2)
        {
            /*
            string url = string.Format(@"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins={0}&destinations={1}&mode={2}",
                                                   address1.Replace(' ', '+').Replace(",", "").Replace("\'", ""),
                                                   address2.Replace(' ', '+').Replace(",", "").Replace("\'", ""),
                                                   infoType);
             */
            string url = string.Format(@"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins={0}&destinations={1}&mode={2}&key={3}",
                                                   address1.Replace(' ', '+').Replace(",", "").Replace("\'", ""),
                                                   address2.Replace(' ', '+').Replace(",", "").Replace("\'", ""),
                                                   mode,
                                                   GOOGLEAPI_KEY);
            // Making and getting the GET response to get the data
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 10000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();

            // Reading and parsing the data
            using (var reader = new StreamReader(resStream, Encoding.UTF8))
            {
                try
                {
                    string value = reader.ReadToEnd();
                    JObject json = JObject.Parse(value);
                    string walkDuration = (string)json["rows"][0]["elements"][0]["duration"]["text"];
                    string walkingDistance = (string)json["rows"][0]["elements"][0]["distance"]["text"];

                    return new Dictionary<string, double?>()
                    {
                        {"Duration", ParseTimespan(walkDuration).TotalMinutes  },
                        {"Distance", ParseDistance(walkingDistance) }
                    };
                }
                catch
                {
                    Console.WriteLine("ERROR!!!!!!");
                    return new Dictionary<string, double?>() {
                        {"Duration", ParseTimespan(Console.ReadLine()).TotalMinutes },
                        {"Distance", ParseDistance(Console.ReadLine()) }
                    };
                }
            }
        }

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
            Dictionary<string, double?> walkingInfo = GetInfoBetweenBuildings("walking", building1.Address, building2.Address);

            // Making changes to the data
            existingDistances.WalkingDistance = walkingInfo["Distance"];
            existingDistances.WalkingDuration = walkingInfo["Duration"];
            existingDistances.CyclingDistance = null;
            existingDistances.CyclingDuration = null;
            existingDistances.TransitDistance = null;
            existingDistances.TransitDuration = null;
            existingDistances.DrivingDistance = null;
            existingDistances.DrivingDuration = null;

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
            Dictionary<string, double?> walkingInfo = GetInfoBetweenBuildings("walking", building1.Address, building2.Address);

            BuildingDistance newBuildingDistance = new BuildingDistance()
            {
                Building = building1,
                Building1 = building2,
                WalkingDistance = walkingInfo["Distance"],
                WalkingDuration = walkingInfo["Duration"],
                CyclingDistance = null,
                CyclingDuration = null,
                TransitDistance = null,
                TransitDuration = null,
                DrivingDistance = null,
                DrivingDuration = null
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
