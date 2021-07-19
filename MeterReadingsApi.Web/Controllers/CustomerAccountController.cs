using System.Threading.Tasks;
using MeterReadingsApi.Interface.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MeterReadingsApi.Web.Controllers
{
    [ApiController]
    [Route("customer-accounts")]
    public class CustomerAccountController : ControllerBase
    {
        private readonly ILogger<CustomerAccountController> _logger;
        public readonly ICustomerAccountService _customerAccountService;

        public CustomerAccountController(ILogger<CustomerAccountController> logger, ICustomerAccountService customerAccountService)
        {
            _logger = logger;
            _customerAccountService = customerAccountService;
        }

        // endpoint to get all customer accounts
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customerAccounts = await _customerAccountService.GetAllAsync();
            
            if (customerAccounts.Length == 0)
            {
                return NoContent();
            }

            return Ok(customerAccounts);
        }

        // endpoint to get a customer account by account ID
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var customerAccount = await _customerAccountService.GetAsync(id, false);

            if (customerAccount == null)
            {
                return NotFound("Customer Account does not exist");
            }

            return Ok(customerAccount);
        }

        // endpoint to get a customer's meter readings
        [HttpGet]
        [Route("{id}/meter-readings")]
        public async Task<IActionResult> GetMeterReadings(int id)
        {
            var customerAccount = await _customerAccountService.GetAsync(id, true);

            if (customerAccount == null)
            {
                return NotFound("Customer Account does not exist");
            }
            else if(customerAccount.MeterReadings.Count == 0)
            {
                return NoContent();
            }

            return Ok(customerAccount.MeterReadings);
        }
    }
}
