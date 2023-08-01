using Memy.Server.Data.Error;
using Memy.Server.Data.Reaction;
using Memy.Server.Filtres;
using Memy.Server.Service;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Mvc;

namespace Memy.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TokenAuthenticationFilter]

    public class ReactionController : ControllerBase
    {
        private readonly Log _logger;
        private readonly IReactionDataBase _reactionData;
        private readonly ReactionService _reactionService;

        public ReactionController(ILogger<ReactionController> logger,
            ILogData data,
                                  IReactionDataBase reactionData)
        {
            _logger = new Log(logger, data);
            _reactionData = reactionData;
            _reactionService = new ReactionService(_reactionData);
        }

        [HttpPost]
        public async Task<IActionResult> PostReaction([FromBody] ReactionModel reaction)
        {
            try
            {
                if (reaction == null)
                {
                    return NoContent();
                }

                string? token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;
                ArgumentNullException.ThrowIfNullOrEmpty(token);
                var result = await _reactionService.SetReaction(reaction, token);

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
