using Memy.Server.Data.Comment;
using Memy.Server.Data.Error;
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
        private readonly CommentService _commentService;
        private readonly Log _logger;
        public CommentController(ILogger<CommentController> logger, ICommentData commentData, ILogData logData)
        {
            _logger = new Log(logger, logData);
            _commentService = new CommentService(commentData);
        }
        #region comment

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
                await _logger.SaveLogError(ex);
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
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region answer

        [HttpGet("answer/{id}")]
        public async Task<IActionResult> GetAnswerComment(int id, int orderTyp = 0)
        {
            try
            {
                if (id < 0)
                {
                    return NoContent();
                }
                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                var result = await _commentService.GetComment(ProcedureName.GetAnswerComment, id, orderTyp, token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthenticationFilter]
        [HttpPost("answer")]
        public async Task<IActionResult> InsertAnswerComment([FromBody] Comment model, int orderTyp = 0)
        {
            try
            {
                if (model == null)
                {
                    return NoContent();
                }

                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;
                var result = await _commentService.InsertComment(ProcedureName.InsertAnswerComment, token, model, orderTyp);
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        [Route("User")]
        [HttpGet]
        public async Task<IActionResult> GetUserComment(string? name, int orderTyp = 0)
        {
            try
            {
                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                if (string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(token))
                {
                    if (string.IsNullOrWhiteSpace(token))
                    {
                        return Unauthorized();
                    }
                    var resultLike = await _commentService.GetLikeUserComment(token, orderTyp);
                    return Ok(resultLike);
                }

                var resultPost = await _commentService.GetUserComment(name, orderTyp);

                if (resultPost == null)
                {
                    return NotFound();
                }

                return Ok(resultPost);
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }

    }
}
