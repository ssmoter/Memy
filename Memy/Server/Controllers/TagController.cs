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
        private readonly ILogger<FileController> _logger;
        private readonly IAddNewFileModel _fileData;
        private readonly FileService _fileService;

        public TagController(IWebHostEnvironment webHostEnvironment,
                             ILogger<FileController> logger,
                             IAddNewFileModel fileData)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _fileData = fileData;
            _fileService = new FileService(webHostEnvironment, _logger, _fileData);
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
                return BadRequest(ex.Message);
            }
        }
    }
}
