using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    [Produces("application/json")]
    [Route("api/Courses")]
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
