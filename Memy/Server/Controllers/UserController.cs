using Memy.Server.Data.Error;
using Memy.Server.Data.User;
using Memy.Server.Filtres;
using Memy.Server.Helper;
using Memy.Server.Service;
using Memy.Shared.Helper;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Memy.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private const string SubjectRegister = "Rejestracja";
        private readonly RegisterService _registerService;
        private readonly UserService _userService;
        private readonly SmtpService _smtpService;
        private readonly IConfiguration _configuration;
        private readonly Log _logger;

        public UserController(ILogger<UserController> logger,
                              IUserData userData,
                              IWebHostEnvironment webHostEnvironment,
                              ILogData data,
                              IConfiguration config)
        {
            _logger = new Log(logger, data);
            _configuration = config;
            _registerService = new RegisterService(userData);
            _userService = new UserService(userData, webHostEnvironment);
            _smtpService = new SmtpService(config);
        }


        // GET api/<UserController>/5
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return NoContent();
                }

                var result = await _userService.GetProfile(name);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }

        [Route("email")]
        [HttpGet]
        [TokenAuthenticationFilter]
        public async Task<IActionResult> GetEmail()
        {
            try
            {
                string? token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                if (!string.IsNullOrWhiteSpace(token))
                {
                    var result = await _userService.GetEmail(token);
                    if (result is not null)
                    {
                        return Ok(result);
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }


        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> PostRegister([FromBody] UserSimple value)
        {
            try
            {
                if (value == null)
                {
                    return NoContent();
                }

                value.Password = ConvertByteString.ConvertToObject<string>(value.Password);

                var result = await _registerService.RegisterUser(value);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    ArgumentNullException.ThrowIfNullOrEmpty(value.Email);
                    await _smtpService.SendEmail(value.Email,
                                                 SubjectRegister,
                                                 EmailBodySchema.Register($"{_configuration.GetValue<string>("UrlPage")}registration-confirmation/{result}"));
                    return Ok();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("registration-confirmation/{value}")]
        public async Task<IActionResult> PostRegisterConfirm(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return NoContent();
                }

                var result = await _registerService.RegisterUserConfirm(value);
                if (result is not null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }



        // PUT api/<UserController>/5
        [TokenAuthenticationFilter]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] object value)
        {
            try
            {
                if (value == null)
                    return NoContent();
                var strValue = value.ToString();

                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;
                Memy.Shared.Helper.MyEnums.UpdateProfile update = (Shared.Helper.MyEnums.UpdateProfile)id;
                switch (update)
                {
                    case Shared.Helper.MyEnums.UpdateProfile.Password:
                        {
                            await _userService.UpdatePassword(token, strValue);
                            return Ok();
                        }
                    case Shared.Helper.MyEnums.UpdateProfile.Name:
                        {
                            var result = await _userService.UpdateName(token, strValue);
                            return Ok(result);
                        }
                    case Shared.Helper.MyEnums.UpdateProfile.Avatar:
                        {
                            await _userService.UpdateAvatar(token, strValue);
                            return Ok();
                        }
                    case Shared.Helper.MyEnums.UpdateProfile.Email:
                        {
                            var result = await _userService.UpdateEmail(token, strValue);
                            return Ok(result);
                        }
                    default:
                        return NoContent();
                }

            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
