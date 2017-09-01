using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UoftTimetableGenerator.DataModels;
using UoftTimetableGenerator.Generator;

namespace UoftTimetableGenerator.WebAPI.Models
{
    public class TimetableRequest
    {
        public string[] CourseCodes { get; set; }
        public Preferences Preferences { get; set; }
        public Restrictions Restrictions { get; set; }
    }
}
