using Memy.Server.Data.Error;
using Memy.Server.Data.File;
using Memy.Server.Filtres;
using Memy.Server.Service;

using Microsoft.AspNetCore.Mvc;

namespace Memy.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Log _logger;
        private readonly IAddNewFileModel _fileData;
        private readonly FileService _fileService;

        public TagController(IWebHostEnvironment webHostEnvironment,
                             ILogger<FileController> logger,
                             ILogData data,
                             IAddNewFileModel fileData)
        {
            _webHostEnvironment = webHostEnvironment;
            _fileData = fileData;
            _logger = new Log(logger, data);
            _fileService = new FileService(webHostEnvironment, _fileData);
        }

        //pobieranie listy tagów
        [HttpGet]
        [TokenAuthenticationFilter]
        public async Task<IActionResult> GetTagList()
        {
            try
            {
                var result = await _fileData.GetTagList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
