using Memy.Server.Data.Error;
using Memy.Server.Data.User;
using Memy.Server.Filtres;
using Memy.Server.Helper;
using Memy.Server.Service;
using Memy.Server.TokenAuthentication;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Memy.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLogController : ControllerBase
    {
        private const string SubjectRegister = "Rejestracja";
        private readonly LoginService _loginService;
        private readonly Log _logger;
        private readonly SmtpService _smtpService;
        private readonly IConfiguration _configuration;

        public UserLogController(ILoginData userData,
                                 ITokenManager tokenManager,
                                 ILogData logData,
                                 ILogger<UserLogController> logger,
                                 IConfiguration configuration)
        {
            _logger = new Log(logger, logData);
            _configuration = configuration;
            _loginService = new LoginService(userData, tokenManager);
            _smtpService = new SmtpService(configuration);
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
                    return Ok(result);
                }
                return NotFound("Nie znaleziono konta");
            }
            catch (ArgumentNullException)
            {
                return NotFound("Nie znaleziono konta");
            }
            catch (UnauthorizedAccessException ex)
            {
                if (value != null)
                {
                    if (!string.IsNullOrWhiteSpace(value.Email))
                    {
                        await _smtpService.SendEmail(value.Email,
                        SubjectRegister,
                                                         EmailBodySchema.Register($"{_configuration.GetValue<string>("UrlPage")}registration-confirmation/{ex.Message}"));
                    }
                }
                return Unauthorized($"W celu zalogowania się potwierdź maila{Environment.NewLine}Wiadomość z potwierdzeniem została przesłana na podanego maila");

            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
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
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
