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

        public ReportedController(IReportedDataBase reportedData, ILogger<ReportedController> logger)
        {
            _reportedService = new ReportedService(reportedData, logger);
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

                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                var result = await _reportedService.SetReactionToFile(reported, token);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
