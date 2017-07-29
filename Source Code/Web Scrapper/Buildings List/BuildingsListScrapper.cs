using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Scrapper;

namespace UoftTimetableGenerator.WebScrapper
{
    internal class BuildingsListScrapper : IWebScrapper
    {
        public static class BuildingsPage
        {
            public static void GetBuildingsList()
            {
                using (BuildingDataContext db = new BuildingDataContext())
                {
                    IWebElement buildingsList = Browser.WebInstance.FindElement(By.ClassName("buildinglist"));
                    IReadOnlyList<IWebElement> buildingElements = buildingsList.FindElements(By.TagName("li"));

                    foreach (IWebElement building in buildingElements)
                    {
                        string[] description = building.FindElement(By.XPath("./dl/dt")).Text.Split('|');
                        string address = building.FindElement(By.XPath("./dl/dd[1]")).Text.Trim();
                        string buildingName = description[0].Trim();
                        string buildingCode = description[1].Trim();

                        Building newBuilding = new Building();
                        newBuilding.Address = address;
                        newBuilding.BuildingCode = buildingCode;
                        newBuilding.BuildingName = buildingName;

                        db.Buildings.InsertOnSubmit(newBuilding);
                    }
                    db.SubmitChanges();
                }
            }
        }

        public void Run()
        {
            Browser.Initialize();
            Browser.WebInstance.Url = "http://map.utoronto.ca/c/buildings";
            BuildingsPage.GetBuildingsList();
            Browser.Close();
        }
    }
}
