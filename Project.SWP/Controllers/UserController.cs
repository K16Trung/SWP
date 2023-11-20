using Microsoft.AspNetCore.Mvc;
using Services.Model;
using Services.Service;
using Services.Service.Interface;

namespace Project.SWP.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _services;
        public UserController(IUserServices services)
        {
            _services = services;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _services.Login(model);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserModel model)
        {
            var result = await _services.CreateUser(model);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers(int pageIndex = 0, int pageSize = 10)
        {
            var result = await _services.GetUsers(pageIndex, pageSize);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] UserModel model)
        {
            var result = await _services.UpdateUser(id, model);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _services.RemoveUser(id);
            return Ok(result);
        }
    }
}
