using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Model;
using Services.Service;
using Services;
using Services.Service.Interface;

namespace Project.SWP.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {

        private readonly IFeedbackService _service;
        public FeedbackController(IFeedbackService services)
        {
            _service = services;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeedbackModel model)
        {
            var result = await _service.CreateFeedback(model);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageIndex = 1, int pageSize = 10)
        {
            var result = await _service.GetFeedbacks(pageIndex, pageSize);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetFeedbackById(id);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] FeedbackModel model)
        {
            var result = await _service.UpdateFeedback(id, model);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _service.DeleteFeedback(id);
            return Ok(result);
        }
    }
}
