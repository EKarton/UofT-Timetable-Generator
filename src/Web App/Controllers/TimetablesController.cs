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
    /// <summary>
    /// A class that generates timetables through HTTP requests
    /// </summary>
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    [Produces("application/json")]
    public class TimetablesController : Controller
    {
        /// <summary>
        /// Generates timetables from UofT courses
        /// Http request: PUT api/timetables/getuofttimetables
        /// </summary>
        /// <param name="request">The request to which timetables to generate</param>
        /// <returns>The generated timetables</returns>
        [HttpPut]
        [Route("GetUoftTimetables")]
        public IActionResult GetUoftTimetables([FromBody] TimetableRequest request)
        {
            if (request == null)
                return BadRequest();

            // Each course code has a length of 10; max 10 courses to put in timetable
            if (request.CourseCodes == null || request.CourseCodes.Length > 100)
                return BadRequest();

            // Check if the preferences / restrictions are set
            if (request.Preferences == null || request.Restrictions == null)
                return BadRequest();

            // Get the courses from the database
            List<Course> courseObjs = new List<Course>();
            foreach (string code in request.CourseCodes)
            {
                Course courseObj = UoftDatabaseService.getService().GetCourseDetails(code);
                if (courseObj == null)
                    return NotFound();
                courseObjs.Add(courseObj);
            }

            // Generate the timetables            
            GeneticScheduler<YearlyTimetable> generator = new GeneticScheduler<YearlyTimetable>(courseObjs, request.Preferences, request.Restrictions)
            {
                NumGenerations = 100,
                PopulationSize = 16,
                MutationRate = 0.1,
                CrossoverRate = 0.6
            };
            List<YearlyTimetable> timetables = generator.GetTimetables();            

            /*
            GreedyGenerator<YearlyTimetable> greedyGenerator = new GreedyGenerator<YearlyTimetable>(courseObjs, request.Preferences, request.Restrictions);
            List<YearlyTimetable> timetables = greedyGenerator.GetTimetables();
            */

            // Convert the timetables to mini timetables (which will be presented to the user)
            List<SimplifiedYearlyTimetable> miniTimetables = new List<SimplifiedYearlyTimetable>();
            for (int i = 0; i < timetables.Count; i++)
                miniTimetables.Add(new SimplifiedYearlyTimetable(timetables[i], "Timetable #" + (i + 1)));                

            return Created("api/timetables/getuofttimetables", miniTimetables);
        }
    }
}
