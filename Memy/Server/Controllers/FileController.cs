using Memy.Server.Data.File;
using Memy.Server.Filtres;
using Memy.Server.Service;
using Memy.Shared.Helper;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Memy.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileController> _logger;
        private readonly IAddNewFileModel _fileData;
        private readonly FileService _fileService;

        public FileController(IWebHostEnvironment webHostEnvironment,
                              ILogger<FileController> logger,
                              IAddNewFileModel fileData)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _fileData = fileData;
            _fileService = new FileService(webHostEnvironment, _logger, _fileData);
        }

        // GET: api/<FileController>
        //pobieranie listy z moelami
        [HttpGet]
        [Route("")]
        [Route("{start}")]
        [Route("{category}/{start}")]
        public async Task<IActionResult> Get(int? start = 1, string? category = "main", int? max = 10, bool? banned = false, string? dateEnd = "empty", string? dateStart = "today")
        {
            try
            {
                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                var result = await _fileService.GetTaskModelsAsync(start, category, max, banned, dateEnd, dateStart, token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //wyświetlanie obrazów
        //GET api/<FileController>/5
        [HttpGet("img/{name}")]
        public async Task<IActionResult> GetImg(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return NotFound();
            }

            try
            {
                var path = Path.Combine(_webHostEnvironment.ContentRootPath,
                    _webHostEnvironment.EnvironmentName, FileRequirements.PatchFolderName, name);
                var image = System.IO.File.OpenRead(path);
                return File(image, $"image/{CheckingFile.GetType(name)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        //pobieranie danych posta
        // POST api/<FileController>
        [TokenAuthenticationFilter]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FileUploadModel model)
        {
            try
            {
                if (model == null)
                {
                    return NoContent();
                }

                if (model.FileUploadStatuses != null)
                {
                    model.FileUploadStatuses = await _fileService.SaveFile(model.FileUploadStatuses);
                }

                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                var id = await _fileService.InsertIntoDataBase(model, token);

                return Ok(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<FileController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FileController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
