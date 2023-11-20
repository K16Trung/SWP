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
    public class PostController : ControllerBase
    {

        private readonly IPostServices _postServices;
        public PostController(IPostServices services)
        {
            _postServices = services;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostModel postModel)
        {
            var result = await _postServices.CreatePost(postModel);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageIndex = 1, int pageSize = 10)
        {
            var result = await _postServices.GetAllPost(pageIndex, pageSize);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id, int pageIndex = 1, int pageSize = 10)
        {
            var result = await _postServices.GetPostById(id, pageIndex, pageSize);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] PostModel postModel)
        {
            var result = await _postServices.UpdatePost(id, postModel);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _postServices.RemovePost(id);
            return Ok(result);
        }
    }
}
