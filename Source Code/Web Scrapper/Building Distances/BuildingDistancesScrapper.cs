using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.WebScrapper
{
    internal class BuildingDistancesScrapper : IWebScrapper
    {
        private static string GOOGLEAPI_KEY = "AIzaSyAna8lO72_VyFrLYjYMTvVgPcvmlU_fCrc"; //"AIzaSyCVCnQ3iBa6S4W8gZkTbwO88d7qhKaGIC8";

        public static TimeSpan ParseTimespan(string time)
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

        public static Distance ParseDistance(string distance)
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

            return new Distance(kilometers, meters, centimeters, millimeters);
        }

        public static BuildingDistances GetDistance(Building building1, Building building2)
        {
            /*
            string url = string.Format(@"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins={0}&destinations={1}&mode=walking&key={2}", 
                                                    building1.Address.Replace(' ', '+').Replace(",", "").Replace("\'", ""),
                                                    building2.Address.Replace(' ', '+').Replace(",", "").Replace("\'", ""), 
                                                    GOOGLEAPI_KEY);
            */
            
            string url = string.Format(@"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins={0}&destinations={1}&mode=walking",
                                                   building1.Address.Replace(' ', '+').Replace(",", "").Replace("\'", ""),
                                                   building2.Address.Replace(' ', '+').Replace(",", "").Replace("\'", ""));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 10000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();

            using (var reader = new StreamReader(resStream, Encoding.UTF8))
            {
                try
                {
                    string value = reader.ReadToEnd();
                    JObject json = JObject.Parse(value);
                    string walkTime = (string)json["rows"][0]["elements"][0]["duration"]["text"];
                    string walkDistance = (string)json["rows"][0]["elements"][0]["distance"]["text"];

                    return new BuildingDistances()
                    {
                        Buildings = new string[] { building1.Code, building2.Code },
                        WalkingTime = ParseTimespan(walkTime.Trim()),
                        WalkingDistance = ParseDistance(walkDistance.Trim())
                    };
                }
                catch
                {
                    Console.WriteLine("ERROR!!!!!!");
                    return new BuildingDistances()
                    {
                        Buildings = new string[] { building1.Code, building2.Code },
                        WalkingTime = ParseTimespan(Console.ReadLine()),
                        WalkingDistance = ParseDistance(Console.ReadLine())
                    };
                }
            }
        }

        public static void ObtainDistancesBetweenBuildings()
        {
            

            //BuildingDistancesList buildingDistList = new BuildingDistancesList();
            //buildingDistList.Distances = distances.ToArray();

            //File.WriteAllText(BUILDINGDISTANCES_FILENAME, JsonConvert.SerializeObject(buildingDistList, Formatting.Indented));
        }

        public void Run()
        {
            BuildingsList buildingList = JsonConvert.DeserializeObject<BuildingsList>(File.ReadAllText(FileLocations.BUILDINGSLIST_FILENAME));

            /*
            for (int i = 127; i < buildingList.Buildings.Length; i++)
            {
                List<BuildingDistances> distances = new List<BuildingDistances>();
                for (int j = i + 1; j < buildingList.Buildings.Length; j++)
                {
                    Building building1 = buildingList.Buildings[i];
                    Building building2 = buildingList.Buildings[j];

                    Console.WriteLine("Performing " + building1.Name + " -> " + building2.Name);
                    BuildingDistances distanceInfo = GetDistance(building1, building2);
                    distances.Add(distanceInfo);

                    Console.WriteLine("Completed " + building1.Name + " -> " + building2.Name);
                }

                BuildingDistancesList buildingDistList = new BuildingDistancesList();
                buildingDistList.Distances = distances.ToArray();

                string filePath = FileLocations.BUILDINGDISTANCES_FOLDER + "\\" + buildingList.Buildings[i].Code + ".json";
                File.WriteAllText(filePath, JsonConvert.SerializeObject(buildingDistList, Formatting.Indented));
                //Console.ReadKey();
            }
            */
            int count = 0;
            for (int i = 0; i < buildingList.Buildings.Length; i++)
            {
                string path = FileLocations.BUILDINGDISTANCES_FOLDER + "\\" + buildingList.Buildings[i].Code.Trim() + ".json";
                if (File.Exists(path) == false)
                    Console.WriteLine(buildingList.Buildings[i].Name + " | " + buildingList.Buildings[i].Code);
                count++;
            }
            Console.WriteLine("Buildings: " + buildingList.Buildings.Length + " == " + count);
            Console.WriteLine(Directory.GetFiles(FileLocations.BUILDINGDISTANCES_FOLDER).Length);

            bool duplicatedCodes = false;
            for (int i = 0; i < buildingList.Buildings.Length; i++)
                for (int j = i + 1; j < buildingList.Buildings.Length; j++)
                    if (buildingList.Buildings[i].Code.Trim() == buildingList.Buildings[j].Code.Trim())
                    {
                        Console.WriteLine(buildingList.Buildings[i].Name + " and " + buildingList.Buildings[j].Name);
                        duplicatedCodes = true;
                    }
            Console.WriteLine(duplicatedCodes);
            Console.ReadKey();
        }
    }
}
