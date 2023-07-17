using Memy.Server.Data.User;
using Memy.Server.Filtres;
using Memy.Server.Helper;
using Memy.Server.Service;
using Memy.Server.TokenAuthentication;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Mvc;

using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Memy.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLogController : ControllerBase
    {
        private readonly ILoginData _userData;
        private readonly ITokenManager _tokenManager;
        private readonly LoginService _loginService;
        private readonly ILogger<UserLogController> _logger;
        public UserLogController(ILoginData userData, ITokenManager tokenManager, ILogger<UserLogController> logger)
        {
            this._userData = userData;
            _tokenManager = tokenManager;
            _logger = logger;
            _loginService = new LoginService(userData, tokenManager, logger);
        }

        // POST api/<UserLogController>
        [HttpPost]
        public async Task<IActionResult> PostLogIn([FromBody] UserSimple? value)
        {
            try
            {
                if (value == null)
                {
                    return NoContent();
                }
                var result = await _loginService.SetToken(value);
                if (result != null)
                {
                    if (string.IsNullOrWhiteSpace(result.Role))
                    {
                        result.Role = null;
                    }

                    var jsonBytes = Memy.Shared.Helper.ConvertByteString.ConvertToString(result);

                    return Ok(jsonBytes);
                }
                return NotFound("Nie znaleziono konta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<UserLogController>/5
        [HttpDelete("{token}")]
        [TokenAuthenticationFilter]
        public async Task<IActionResult> LogOut(string? token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    return NoContent();
                }
                var result = await _loginService.DeleteToken(token);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    return Ok(result);
                }
                return NotFound("Nie znaleziono tokenu");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
