using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UoftTimetableGenerator.WebAPI
{
    [Produces("application/json")]
    [Route("api/courses")]
    public class CoursesController : Controller
    {
        // GET: api/courses
        [HttpGet]
        public IActionResult GetUoftCourses()
        {
            return Ok();
        }

        // GET: api/courses/mat137
        [HttpGet("{course}")]
        public IActionResult GetUoftCourseInfo(string courseCode)
        {
            return Ok();
        }
    }
}
