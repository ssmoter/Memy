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
        private readonly IUserData _userData;
        private readonly ITokenManager _tokenManager;
        private readonly LoginService _loginService;
        private readonly ILogger<UserLogController> _logger;
        public UserLogController(IUserData userData, ITokenManager tokenManager, ILogger<UserLogController> logger)
        {
            this._userData = userData;
            _tokenManager = tokenManager;
            _logger = logger;
            _loginService = new LoginService(userData, tokenManager, logger);
        }


        // GET: api/<UserLogController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserLogController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost("Register")]
        public async Task<IActionResult> PostRegister([FromBody] int? a)
        {
            try
            {

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
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
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented, JsonSettings.JsonSerializerSettings());
                    return Ok(json);
                }
                return NotFound("Nie znaleziono konta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<UserLogController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] UserSimple value)
        {

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
