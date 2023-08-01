using Memy.Server.Data.Admin;
using Memy.Server.Data.Error;
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
        private readonly FileAdminService _fileAdminService;
        private readonly Log _logger;

        public FileAdminController(IWebHostEnvironment webHostEnvironment,
                              ILogger<FileAdminController> logger,
                              ILogData data,
                              IAdminData adminData)
        {
            _logger = new Log(logger, data);
            _fileAdminService = new FileAdminService(webHostEnvironment, adminData);
        }



        [HttpPut]
        [Route("{id}/category")]
        public async Task<IActionResult> Category(int id, string category, [FromBody] ReportedMessagesModel? reportedMessagesModel)
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
                await _fileAdminService.UpdateCategoryFile(id, category, token, reportedMessagesModel);

                return Ok();
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}/ban")]
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
                if (!string.IsNullOrWhiteSpace(token))
                {
                    await _fileAdminService.BanFile(id, token, reportedMessagesModel);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}/delete")]
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
                if (!string.IsNullOrWhiteSpace(token))
                {
                    await _fileAdminService.DeleteFile(id, token, reportedMessagesModel);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
