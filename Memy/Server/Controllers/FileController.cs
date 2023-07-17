using Memy.Server.Data.File;
using Memy.Server.Filtres;
using Memy.Server.Service;
using Memy.Shared.Helper;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Mvc;

using System.Xml.Linq;

using static Memy.Shared.Helper.MyEnums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Memy.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly FileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileController(IWebHostEnvironment webHostEnvironment,
                              ILogger<FileController> logger,
                              IAddNewFileModel fileData)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _fileService = new FileService(webHostEnvironment, _logger, fileData);
        }

        // GET: api/<FileController>
        //pobieranie listy z moelami
        [HttpGet]
        [Route("")]
        [Route("{start}")]
        [Route("{category}/{start}")]
        public async Task<IActionResult> Get(int? start = 1, string? category = "main", int? max = 10, bool? banned = false, string? dateEnd = "empty", string? dateStart = "today", int orderTyp = 0)
        {
            try
            {
                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                var result = await _fileService.GetTaskModelsAsync(start, category, max, banned, dateEnd, dateStart, orderTyp, token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("obj/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound();
                }
                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                var result = await _fileService.GetTaskModelAsync(id, token);

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

        [HttpGet("video/{name}")]
        public async Task<IActionResult> GetVideo(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return NotFound();
            }

            try
            {
                var path = Path.Combine(_webHostEnvironment.ContentRootPath,
                    _webHostEnvironment.EnvironmentName, FileRequirements.PatchFolderName, name);

                return PhysicalFile(path, "application/octet-stream", enableRangeProcessing: true);
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

        //pobieranie plików poszczególnych user
        [Route("User")]
        [HttpGet]
        public async Task<IActionResult> GetUserPost(string? name, int start = 0, int max = 10, int orderTyp = 0, bool banned = false)
        {
            try
            {
                var token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                if (string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(token))
                {
                    if (string.IsNullOrWhiteSpace(token))
                    {
                        return Unauthorized();
                    }
                    var resultLike = await _fileService.GetLikeUserTasksModel(token, start, max, orderTyp);
                    return Ok(resultLike);
                }

                var resultPost = await _fileService.GetUserTasksModel(name,start, max, orderTyp, banned);

                if (resultPost == null)
                {
                    return NotFound();
                }

                return Ok(resultPost);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/<FileController>/5
        [HttpPut("{id}")]
        [TokenAuthenticationFilter]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FileController>/5
        [HttpDelete("{id}")]
        [TokenAuthenticationFilter]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
