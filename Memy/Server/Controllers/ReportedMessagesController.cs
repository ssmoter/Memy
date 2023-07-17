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

        public ReportedMessagesController(ILogger<ReportedMessagesController> logger, IReportedMessagesData messages)
        {
            _messagesService = new ReportedMessagesService(logger, messages);
        }

        // GET: api/<MessagesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;
                var result = await _messagesService.GetMessages(token);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ReportedMessagesModel messagesModel)
        {
            try
            {
                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;
                var result = await _messagesService.UpdateMessages(token, messagesModel);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
