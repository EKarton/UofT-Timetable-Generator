using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using UoftTimetableGenerator.DataModels;

namespace UoftTimetableGenerator.WebAPI
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class CoursesController : Controller
    {
        // GET: api/courses
        [HttpGet]
        public IActionResult GetUoftCourses([FromQuery] string query)
        {
            if (query == null)
                return BadRequest();

            List<Course> courses = UoftDatabaseService.GetCourses(query, UoftDatabaseService.CourseQueryType.CourseCode);
            return Ok(courses);
        }
    }
}
