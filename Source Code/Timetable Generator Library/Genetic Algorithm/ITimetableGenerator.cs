using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    public interface ITimetableGenerator
    {
        List<Timetable> GetTimetables(string[] filters);
    }
}
