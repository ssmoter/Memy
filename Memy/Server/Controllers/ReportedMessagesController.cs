using Memy.Server.Data.Error;
using Memy.Server.Data.User;
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
    public class ReportedMessagesController : ControllerBase
    {

        private readonly ReportedMessagesService _messagesService;
        private readonly Log _logger;

        public ReportedMessagesController(ILogger<ReportedMessagesController> logger,
                                          ILogData logData,
                                          IReportedMessagesData messages)
        {
            _logger = new Log(logger, logData);
            _messagesService = new ReportedMessagesService(messages);
        }

        // GET: api/<MessagesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                string? token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var result = await _messagesService.GetMessages(token);
                    return Ok(result);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ReportedMessagesModel messagesModel)
        {
            try
            {
                string? token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var result = await _messagesService.UpdateMessages(token, messagesModel);
                    return Ok(result);
                }
                return NoContent();

            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }

    }
}
