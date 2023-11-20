using Microsoft.AspNetCore.Mvc;
using Services.Model;
using Services.Service.Interface;

namespace Project.SWP.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        public PaymentController(IOrderServices services)
        {
            _orderServices = services;
        }

        [HttpPost]
        public async Task<IActionResult> Payment(int? orderId, PaymentModel paymentModel)
        {
            return Ok(orderId is null || orderId.Value == 0
                ? await _orderServices.Payment(paymentModel)
                : await _orderServices.Payment(orderId.Value, paymentModel));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, int pageSize = 10)
        {
            var result = await _orderServices.GetPayments(pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _orderServices.GetPaymentById(id);
            return Ok(result);
        }
    }
}
