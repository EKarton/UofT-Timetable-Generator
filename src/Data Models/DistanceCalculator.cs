using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace UoftTimetableGenerator.DataModels
{
    public class BuildingDistanceCalculator
    {
        private string[] API_KEYS = { "", "AIzaSyAna8lO72_VyFrLYjYMTvVgPcvmlU_fCrc", "AIzaSyCVCnQ3iBa6S4W8gZkTbwO88d7qhKaGIC8" };

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
        /// Return the URL to the Google API.
        /// If the API key is empty, then it will use the URL to the Google API without the URL.
        /// </summary>
        /// <param name="address1">The address of the origin</param>
        /// <param name="address2">The address of the target</param>
        /// <param name="mode">The mode of transportation</param>
        /// <param name="apiKey">THe API key</param>
        /// <returns></returns>
        private string GetURL(string address1, string address2, string mode, string apiKey)
        {
            if (apiKey == "")
            {
                return string.Format(@"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins={0}&destinations={1}&mode={2}",
                                                       address1.Replace(' ', '+').Replace(",", "").Replace("\'", ""),
                                                       address2.Replace(' ', '+').Replace(",", "").Replace("\'", ""),
                                                       mode);
            }
            else
            {
                return string.Format(@"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins={0}&destinations={1}&mode={2}&key={3}",
                                                   address1.Replace(' ', '+').Replace(",", "").Replace("\'", ""),
                                                   address2.Replace(' ', '+').Replace(",", "").Replace("\'", ""),
                                                   mode,
                                                   apiKey);
            }
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
        public DataModels.BuildingDistance GetInfoBetweenBuildings(string mode, string address1, string address2)
        {
            int curAPIKeyIndex = 0;

            while (curAPIKeyIndex < API_KEYS.Length)
            {
                string url = GetURL(address1, address2, mode, API_KEYS[curAPIKeyIndex]);

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

                        return new DataModels.BuildingDistance()
                        {
                            WalkDuration = ParseTimespan(walkDuration).TotalMinutes,
                            WalkingDistance = ParseDistance(walkingDistance)
                        };
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("ERROR! Trying different API key...");
                    }
                }
                curAPIKeyIndex++;
            }
            return null;
        }
    }
}