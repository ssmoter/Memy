using Memy.Server.Data;
using Memy.Server.Data.Error;
using Memy.Server.Data.Reported;
using Memy.Server.Filtres;
using Memy.Server.Service;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Memy.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TokenAuthenticationFilter]
    public class ReportedController : ControllerBase
    {
        private readonly ReportedService _reportedService;
        private readonly Log _logger;

        public ReportedController(IReportedDataBase reportedData,
            ILogData logData,
                                  ILogger<ReportedController> logger)
        {
            _logger = new Log(logger, logData); 
            _reportedService = new ReportedService(reportedData);
        }

        [HttpPost]
        public async Task<IActionResult> PostReport([FromBody] ReportedModel reported)
        {
            try
            {
                if (reported == null)
                {
                    return NoContent();
                }

                string? token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                ArgumentNullException.ThrowIfNullOrEmpty(token);
                var result = await _reportedService.SetReactionToFile(reported, token);

                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
