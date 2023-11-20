using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Model;
using Services.Service;
using Services.Service.Interface;

namespace Project.SWP.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseServices _courseServices;
        public CourseController(ICourseServices services)
        {
            _courseServices = services;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseModel courseModel)
        {
            var result = await _courseServices.CreateCourse(courseModel);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageIndex = 1, int pageSize = 10)
        {
            var result = await _courseServices.GetAllCourse(pageIndex, pageSize);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id, int pageIndex = 1, int pageSize = 10)
        {
            var result = await _courseServices.GetCourseById(id, pageIndex, pageSize);
            return Ok(result);
        }
        [HttpGet("{coursename}")]
        public async Task<IActionResult> GetCourByName(string coursename, int pageIndex = 1, int pageSize = 10)
        {
            var result = await _courseServices.GetCourseByName(coursename, pageIndex, pageSize);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] CourseModel courseModel)
        {
            var result = await _courseServices.UpdateCourse(id, courseModel);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _courseServices.RemoveCourse(id);
            return Ok(result);
        }
    }
}
