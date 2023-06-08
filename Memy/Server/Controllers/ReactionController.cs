using Memy.Server.Data.Reaction;
using Memy.Server.Service;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Mvc;

namespace Memy.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ReactionController> _logger;
        private readonly IReactionDataBase _reactionData;
        private readonly ReactionService _reactionService;

        public ReactionController(IWebHostEnvironment webHostEnvironment,
                              ILogger<ReactionController> logger,
                              IReactionDataBase reactionData)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _reactionData = reactionData;
            _reactionService = new ReactionService(_reactionData, _logger);
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

                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                var result = await _reactionService.SetReaction(reaction, token);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
