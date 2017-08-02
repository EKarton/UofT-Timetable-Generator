using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataContext;

namespace UoftTimetableGenerator.WebScrapper
{
    class BuildingDistancePopulator
    {
        private static string GOOGLEAPI_KEY = "AIzaSyAna8lO72_VyFrLYjYMTvVgPcvmlU_fCrc"; //"AIzaSyCVCnQ3iBa6S4W8gZkTbwO88d7qhKaGIC8";

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

        private Dictionary<string, double?> GetInfoBetweenBuildings(string infoType, string address1, string address2)
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
                                                   infoType,
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

        public void UpdateBuildingDistanceInfo()
        {
            using (UoftDataContext db = new UoftDataContext())
            {
                List<BuildingDistance> rows = db.BuildingDistances.ToList();
                foreach (BuildingDistance row in rows)
                {
                    Building building1 = row.Building;
                    Building building2 = row.Building1;

                    Console.WriteLine("Attempting to update data " + building1.BuildingCode + " -> " + building2.BuildingCode);

                    // Getting distance info (like walk durations, etc)
                    Dictionary<string, double?> walkingInfo = GetInfoBetweenBuildings("walking", building1.Address, building2.Address);

                    // Making changes to the data
                    row.WalkingDistance = walkingInfo["Distance"];
                    row.WalkingDuration = walkingInfo["Duration"];
                    row.CyclingDistance = null;
                    row.CyclingDuration = null;
                    row.TransitDistance = null;
                    row.TransitDuration = null;
                    row.VehicleDistance = null;
                    row.VehicleDuration = null;

                    db.SubmitChanges();
                    Console.WriteLine("Successful! Has updated data for " + building1.BuildingCode + " -> " + building2.BuildingCode);
                }
            }
        }

        public void InsertBuildingDistancesToDatabase(bool redoEntireTable)
        {
            using (UoftDataContext db = new UoftDataContext())
            {
                List<Building> buildings = db.Buildings.ToList();
                for (int i = 0; i < buildings.Count; i++)
                    for (int j = i + 1; j < buildings.Count; j++)
                    {
                        Building building1 = buildings[i];
                        Building building2 = buildings[j];
                        Console.WriteLine("Attempting to insert data " + building1.BuildingCode + " -> " + building2.BuildingCode);

                        // Getting pre-existing buildings
                        var existingData = from row in db.BuildingDistances
                                           where (row.BuildingID1 == building1.Id && row.BuildingID2 == building2.Id) ||
                                                       (row.BuildingID1 == building2.Id && row.BuildingID2 == building1.Id)
                                           select row;

                        // If the user doesnt want to rewrite the existing rows
                        if (!redoEntireTable && existingData.Count() == 1)
                            continue;

                        // If there is a discreptency with the data (there is pre-existing data)
                        else if (existingData.Count() >= 2)
                        {
                            // Delete those pre-existing data
                            db.BuildingDistances.DeleteAllOnSubmit(existingData);
                            db.SubmitChanges();
                        }

                        Console.WriteLine("Getting data " + building1.Address + " -> " + building2.Address);
                        //Console.ReadKey();

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
                            VehicleDistance = null,
                            VehicleDuration = null
                        };

                        Console.WriteLine("Inserting new data " + building1.BuildingCode + " -> " + building2.BuildingCode);
                        db.BuildingDistances.InsertOnSubmit(newBuildingDistance);
                        db.SubmitChanges();
                    }
            }
        }
    }
}
