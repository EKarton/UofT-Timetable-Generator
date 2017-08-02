using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UoftTimetableGenerator.DataModels;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    public class TimetablesController : Controller
    {
        // GET api/timetables?courses=mat137,csc148
        [HttpGet]
        public IActionResult GetUoftTimetables(string courses)
        {
            // Each course code has a length of 10; max 10 courses to put in timetable
            if (courses.Length > 100)
                return BadRequest();

            // Get the courses from the database
            List<Course> courseObjs = new List<Course>();
            foreach (string code in courses.Split(','))
            {
                Course courseObj = UoftDatabaseService.GetCourse(code);
                if (courseObj == null)
                    return BadRequest();
                courseObjs.Add(courseObj);
            }

            List<Section[]> requiredSections = new List<Section[]>();
            foreach (Course c in courseObjs)
                foreach (Activity a in c.Activities)
                    requiredSections.Add(a.Sections.ToArray());



            return Ok();
        }

        // POST api/timetables
        [HttpPost]
        public IActionResult GetTimetables([FromBody] Course[] courses)
        {
            return Ok();
        }
    }
}
