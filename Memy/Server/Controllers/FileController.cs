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
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger _logger;
        private readonly IAddNewFileModel _fileData;
        private readonly FileService _fileService;

        public FileController(IWebHostEnvironment webHostEnvironment, ILogger logger, IAddNewFileModel fileData)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _fileData = fileData;
            _fileService = new FileService(webHostEnvironment, _logger, _fileData);
        }

        // GET: api/<FileController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FileController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

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

                await _fileService.InsertIntoDataBase(model, token);

                return Ok();
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
