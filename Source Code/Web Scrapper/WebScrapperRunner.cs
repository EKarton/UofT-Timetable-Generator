using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.WebScrapper
{
    internal class WebScrapperRunner
    {
        static void Main(string[] args)
        {
            IWebScrapper webScrapper = new BuildingDistancesScrapper();
            webScrapper.Run();
        }
    }
}
