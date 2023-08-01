using Memy.Server.Data.Error;
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
        private readonly FileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Log _logger;
        public FileController(IWebHostEnvironment webHostEnvironment,
                              ILogger<FileController> logger,
                              ILogData data,
                              IAddNewFileModel fileData)
        {
            _logger = new Log(logger, data);
            _webHostEnvironment = webHostEnvironment;
            _fileService = new FileService(webHostEnvironment, fileData);
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
                await _logger.SaveLogError(ex);
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
                    return NoContent();
                }
                string? token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                var result = await _fileService.GetTaskModelAsync(id, token);
                return Ok(result);

            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
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
                await _logger.SaveLogError(ex);
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
                await _logger.SaveLogError(ex);
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

                string? token = Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value;

                if (!string.IsNullOrWhiteSpace(token))
                {
                    var id = await _fileService.InsertIntoDataBase(model, token);
                    return Ok(id);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
                return BadRequest(ex.Message);
            }
        }

        //pobieranie plików poszczególnych user
        [Route("User")]
        [HttpGet]
        public async Task<IActionResult> GetUserPost(string name, int start = 0, int max = 10, int orderTyp = 0, bool banned = false)
        {
            try
            {
                string? token = Request.Headers.FirstOrDefault(x => x.Key == Headers.Authorization).Value;


                if (string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(token))
                {
                    var resultLike = await _fileService.GetLikeUserTasksModel(token, start, max, orderTyp);
                    return Ok(resultLike);
                }

                var resultPost = await _fileService.GetUserTasksModel(name, start, max, orderTyp, banned);

                if (resultPost == null)
                {
                    return NotFound();
                }

                return Ok(resultPost);
            }
            catch (Exception ex)
            {
                await _logger.SaveLogError(ex);
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
                await Task.Run(() => { Task.Delay(1); });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
