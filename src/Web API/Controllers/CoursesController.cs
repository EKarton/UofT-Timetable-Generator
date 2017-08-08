using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace UoftTimetableGenerator.WebAPI
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class CoursesController : Controller
    {
        // GET: api/courses
        [HttpGet]
        public IActionResult GetUoftCourses()
        {
            return Ok();
        }
    }
}
