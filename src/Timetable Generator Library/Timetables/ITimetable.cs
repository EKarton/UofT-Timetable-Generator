using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.Generator
{
    public interface ITimetable
    {
        bool AddSection(Section section);
        bool Contains(Section section);
        bool DeleteSection(Section section);
        bool DoesSectionFit(Section section);
        List<Section> GetSections();
        ITimetable MakeCopy();
    }
}
