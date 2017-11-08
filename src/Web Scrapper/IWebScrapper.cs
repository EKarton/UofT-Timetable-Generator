using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.WebScrapper
{
    interface IWebScrapper
    {
        void CreateDatabase();
        void InsertData();
        void ClearData();
        void UpdateData();
    }
}
