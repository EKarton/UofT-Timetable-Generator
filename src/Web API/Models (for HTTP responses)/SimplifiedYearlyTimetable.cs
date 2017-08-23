using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;
using UoftTimetableGenerator.Generator;

namespace UoftTimetableGenerator.WebAPI.Models
{
    internal class SimplifiedYearlyTimetable
    {
        public SimplifiedYearlyTimetable(YearlyTimetable existingTimetable, string name)
        {
            Name = name;

            // Populating fall-timetable blocks
            foreach (Section section in existingTimetable.GetFallSections())
                foreach (Session session in section.Sessions)
                {
                    char term = session.Section.Activity.Course.Term[0];
                    SimplifiedTimetableBlock block = new SimplifiedTimetableBlock(session, term);
                    FallTimetableBlocks.Add(block);
                }

            // Getting winter-timetable blocks
            foreach (Section section in existingTimetable.GetWinterSections())
                foreach (Session session in section.Sessions)
                {
                    char term = session.Section.Activity.Course.Term[0];
                    SimplifiedTimetableBlock block = new SimplifiedTimetableBlock(session, term);
                    WinterTimetableBlocks.Add(block);
                }
        }

        public string Name { get; set; }
        public List<SimplifiedTimetableBlock> FallTimetableBlocks = new List<SimplifiedTimetableBlock>();
        public List<SimplifiedTimetableBlock> WinterTimetableBlocks = new List<SimplifiedTimetableBlock>();
    }
}
