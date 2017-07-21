using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.WebScrapper
{
    internal class BuildingsListScrapper : IWebScrapper
    {
        public static class BuildingsPage
        {
            public static BuildingsList GetBuildingsList()
            {
                IWebElement buildingsList = Browser.WebInstance.FindElement(By.ClassName("buildinglist"));
                IReadOnlyList<IWebElement> buildingElements = buildingsList.FindElements(By.TagName("li"));

                List<Building> buildings = new List<Building>();

                foreach (IWebElement building in buildingElements)
                {
                    string[] description = building.FindElement(By.XPath("./dl/dt")).Text.Split('|');
                    string address = building.FindElement(By.XPath("./dl/dd[1]")).Text.Trim();
                    string buildingName = description[0].Trim();
                    string buildingCode = description[1].Trim();

                    Building newBuilding = new Building()
                    {
                        Name = buildingName,
                        Address = address,
                        Code = buildingCode
                    };

                    buildings.Add(newBuilding);
                }

                return new BuildingsList() { Buildings = buildings.ToArray() };
            }
        }

        public void Run()
        {
            Browser.Initialize();
            Browser.WebInstance.Url = "http://map.utoronto.ca/c/buildings";
            BuildingsList list = BuildingsPage.GetBuildingsList();
            File.WriteAllText(FileLocations.BUILDINGSLIST_FILENAME, JsonConvert.SerializeObject(list, Formatting.Indented));
            Browser.Close();
        }
    }
}
