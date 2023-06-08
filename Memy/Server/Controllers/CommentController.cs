using Memy.Server.Data.Comment;
using Memy.Server.Filtres;
using Memy.Server.Helper;
using Memy.Server.Service;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Mvc;

namespace Memy.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly CommentService _commentService;
        private readonly ICommentData _commentData;
        public CommentController(ILogger<CommentController> logger, ICommentData commentData)
        {
            this._logger = logger;
            _commentData = commentData;
            _commentService = new CommentService(logger, commentData);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id, int orderTyp = 0)
        {
            try
            {
                if (id < 0)
                {
                    return NoContent();
                }
                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                var result = await _commentService.GetComment(ProcedureName.GetComment, id, orderTyp, token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthenticationFilter]
        [HttpPost]
        public async Task<IActionResult> InsertComment([FromBody] Comment model, int orderTyp = 0)
        {
            try
            {
                if (model == null)
                {
                    return NoContent();
                }

                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;
                var result = await _commentService.InsertComment(ProcedureName.InsertComment, token, model, orderTyp);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }


    }
}
