using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataContext;
using UoftTimetableGenerator.WebScrapper;

namespace Web_Scrapper
{
    class BuildingListPopulator
    {
        public void UpdateBuildingsInfo()
        {
            using (UoftDataContext db = new UoftDataContext())
            {
                db.Buildings.DeleteAllOnSubmit(db.Buildings);
                db.SubmitChanges();
            }
            InsertBuildingsList();
        }

        public void InsertBuildingsList()
        {
            Browser.Initialize();
            Browser.WebInstance.Url = "http://map.utoronto.ca/c/buildings";

            using (UoftDataContext db = new UoftDataContext())
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

            Browser.Close();
        }
    }
}
