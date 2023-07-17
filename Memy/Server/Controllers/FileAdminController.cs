using Memy.Server.Data.Admin;
using Memy.Server.Data.File;
using Memy.Server.Filtres;
using Memy.Server.Service;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Memy.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AdminAuthenticationFilter]
    public class FileAdminController : ControllerBase
    {
        private readonly ILogger<FileAdminController> _logger;
        private readonly FileAdminService _fileAdminService;

        public FileAdminController(IWebHostEnvironment webHostEnvironment,
                              ILogger<FileAdminController> logger,
                              IAdminData adminData)
        {
            _logger = logger;
            _fileAdminService = new FileAdminService(webHostEnvironment, _logger, adminData);
        }



        [HttpPut]
        [Route("{id}/Category")]
        public async Task<IActionResult> Category(int id, [FromBody] ReportedMessagesModel? reportedMessagesModel)
        {
            try
            {
                if (id < 0)
                {
                    NotFound();
                }
                if (reportedMessagesModel == null)
                {
                    return NoContent();
                }
                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;
                await _fileAdminService.CategoryFile(id, token, reportedMessagesModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}/Ban")]
        public async Task<IActionResult> Ban(int id, [FromBody] ReportedMessagesModel? reportedMessagesModel)
        {
            try
            {
                if (id < 0)
                {
                    NotFound();
                }
                if (reportedMessagesModel == null)
                {
                    return NoContent();
                }
                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;
                await _fileAdminService.BanFile(id, token, reportedMessagesModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}/Delete")]
        public async Task<IActionResult> Delete(int id, [FromBody] ReportedMessagesModel? reportedMessagesModel)
        {
            try
            {
                if (id < 0)
                {
                    NotFound();
                }
                if (reportedMessagesModel == null)
                {
                    return NoContent();
                }
                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;
                await _fileAdminService.DeleteFile(id, token, reportedMessagesModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
