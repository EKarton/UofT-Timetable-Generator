using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace UoftTimetableGenerator.WebScrapper
{
    internal class WebScrapperRunner
    {
        /// <summary>
        /// Runs the web scrapper
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            CourseSchedulePopulator p = new CourseSchedulePopulator();
            p.CreateCourseSchedules(true);

            //BuildingListPopulator p = new BuildingListPopulator();
            //p.RedoBuildingList();

            Console.ReadKey();
        }
    }
}
