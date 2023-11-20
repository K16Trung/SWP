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
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        public OrderController(IOrderServices services)
        {
            _orderServices = services;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderModel orderModel)
        {
            var result = await _orderServices.CreateOrder(orderModel);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, int pageSize = 10)
        {
            var result = await _orderServices.GetOrders(pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderServices.GetOrderById(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] OrderModel orderModel)
        {
            var result = await _orderServices.UpdateOrder(id, orderModel);
            return Ok(result);
        }
    }
}
