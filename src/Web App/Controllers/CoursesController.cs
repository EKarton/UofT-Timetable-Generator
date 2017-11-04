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
    /// <summary>
    /// A class used to handle UofT course-related HTTP requests
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class CoursesController : Controller
    {
        /// <summary>
        /// Get information about a particular course code
        /// Http request: GET: api/courses
        /// Note that the Course object returned will not contain information about 
        /// the activities associated with this course
        /// </summary>
        /// <param name="query">The course code</param>
        /// <returns>Information about this course</returns>
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
