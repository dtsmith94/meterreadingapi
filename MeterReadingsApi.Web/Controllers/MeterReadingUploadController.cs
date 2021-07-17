using System.IO;
using System.Text;
using System.Threading.Tasks;
using MeterReadingsApi.Interface.Service.Services;
using MeterReadingsApi.Model.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MeterReadingsApi.Web.Controllers
{
    [ApiController]
    [Route("meter-reading-uploads")]
    public class MeterReadingUploadController : ControllerBase
    {
        private readonly ILogger<MeterReadingUploadController> _logger;
        public readonly ICustomerAccountService _customerAccountService;
        private readonly IMeterReadingService _meterReadingService;

        public MeterReadingUploadController(ILogger<MeterReadingUploadController> logger, ICustomerAccountService customerAccountService, IMeterReadingService meterReadingService)
        {
            _logger = logger;
            _customerAccountService = customerAccountService;
            _meterReadingService = meterReadingService;
        }

        // endpoint to get all customer accounts (for the purpose of checking the data seeded ok)
        [HttpGet]
        public async Task<CustomerAccount[]> Get()
        {
            return await _customerAccountService.GetAllAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);

            var csvRawText = await reader.ReadToEndAsync();

            if (string.IsNullOrEmpty(csvRawText))
            {
                return BadRequest("No CSV data was provided in the request");
            }

            var result = await _meterReadingService.ProcessNewReadings(csvRawText);

            if (result.SuccessfulUploads == 0)
            {
                return BadRequest(result);
            }

            return Ok();

        }
    }
}
