using System.IO;
using System.Text;
using System.Threading.Tasks;
using MeterReadingsApi.Interface.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MeterReadingsApi.Web.Controllers
{
    [ApiController]
    [Route("meter-reading-uploads")]
    public class MeterReadingUploadController : ControllerBase
    {
        private readonly ILogger<MeterReadingUploadController> _logger;
        private readonly IMeterReadingService _meterReadingService;

        public MeterReadingUploadController(ILogger<MeterReadingUploadController> logger, IMeterReadingService meterReadingService)
        {
            _logger = logger;
            _meterReadingService = meterReadingService;
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

            return Created(Request.Scheme + "://" + Request.Host + Request.Path, result);
        }
    }
}
