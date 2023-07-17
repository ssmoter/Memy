using Memy.Server.Data.User;
using Memy.Server.Filtres;
using Memy.Server.Service;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Memy.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly RegisterService _registerService;
        private readonly UserService _userService;

        public UserController(ILogger<UserController> logger, IUserData userData, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _registerService = new RegisterService(logger);
            _userService = new UserService(logger, userData, webHostEnvironment);
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
                _logger.LogError(ex.Message);
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
                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                var result = await _userService.GetEmail(token);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string value)
        {
            try
            {
                if (value == null)
                {
                    return NoContent();
                }



                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                _logger.LogError(ex.Message);
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
