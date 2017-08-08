using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UoftTimetableGenerator.DataModels;
using UoftTimetableGenerator.Generator;
using UoftTimetableGenerator.WebAPI.Models;
using Microsoft.AspNetCore.Cors;

namespace UoftTimetableGenerator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    [Produces("application/json")]
    public class TimetablesController : Controller
    {
        // PUT api/timetables
        [HttpPut]
        [Route("GetUoftTimetables")]
        public IActionResult GetUoftTimetables([FromBody] string[] courseCodes)
        {
            // Each course code has a length of 10; max 10 courses to put in timetable
            if (courseCodes.Length > 100)
                return BadRequest();

            // Get the courses from the database
            List<Course> courseObjs = new List<Course>();
            foreach (string code in courseCodes)
            {
                Course courseObj = UoftDatabaseService.GetCourse(code);
                if (courseObj == null)
                    return NotFound();
                courseObjs.Add(courseObj);
            }

            // Generate the timetables
            GAGenerator generator = new GAGenerator(courseObjs)
            {
                NumGenerations = 100,
                PopulationSize = 16,
                MutationRate = 0.01,
                CrossoverRate = 0.9,
                CrossoverType = "Uniform Crossover"
            };

            List<YearlyTimetable> timetables = generator.GetTimetables();

            // Convert the timetables to mini timetables (which will be presented to the user)
            List<SimplifiedYearlyTimetable> miniTimetables = new List<SimplifiedYearlyTimetable>();
            foreach (YearlyTimetable t in timetables)
                miniTimetables.Add(new SimplifiedYearlyTimetable(t));

            return Created("api/timetables/getuofttimetables", miniTimetables);
        }

        // PUT api/timetables
        [HttpPut]
        [Route("GetTimetables")]
        public IActionResult GetTimetables([FromBody] Course[] courses)
        {
            // Max 12 courses (a full yr course counts as 1 course)
            if (courses.Length > 12)
                return BadRequest();

            // Generate the timetables
            GAGenerator generator = new GAGenerator(courses.ToList())
            {
                NumGenerations = 100,
                PopulationSize = 16,
                MutationRate = 0.01,
                CrossoverRate = 0.9,
                CrossoverType = "Uniform Crossover"
            };

            List<YearlyTimetable> timetables = generator.GetTimetables();

            // Convert the timetables to mini timetables (which will be presented to the user)
            List<SimplifiedYearlyTimetable> miniTimetables = new List<SimplifiedYearlyTimetable>();
            foreach (YearlyTimetable t in timetables)
                miniTimetables.Add(new SimplifiedYearlyTimetable(t));

            return Created("api/timetables/gettimetables", miniTimetables);
        }
    }
}
