using Memy.Server.Data.File;
using Memy.Server.Helper;
using Memy.Shared.Helper;
using Memy.Shared.Model;

namespace Memy.Server.Service
{
    public class FileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger _logger;
        private readonly IAddNewFileModel _fileData;

        public FileService(IWebHostEnvironment webHostEnvironment, ILogger logger, IAddNewFileModel fileData)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _fileData = fileData;
        }

        //Zapisywanie plików na dysku
        public async Task<FileUploadStatus[]> SaveFile(FileUploadStatus[] model)
        {
            try
            {
                if (model != null)
                {
                    if (model.Length != 0)
                    {
                        var status = new FileUploadStatus();
                        for (int i = 0; i < model.Length; i++)
                        {
                            if (i >= Memy.Shared.Helper.FileRequirements.MaxNumberOfFiles)
                                break;
                            status = await CheckingFile.CorrectData(model[i], _webHostEnvironment);
                            model[i] = status;
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        //zapisanie do bazy danych
        public async Task<int> InsertIntoDataBase(FileUploadModel model, string token)
        {
            try
            {
                if (model.Categories == Categories.Main)
                {
                    model.Categories = null;
                }
                if (string.IsNullOrWhiteSpace(model.Categories))
                {
                    model.Categories = Categories.Waiting;
                }
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(model, Newtonsoft.Json.Formatting.Indented, JsonSettings.JsonSerializerSettings());
                var id = await _fileData.InsertFullFile(json, token);

                return id;
            }
            catch (Exception ex)
            {
                for (int i = 0; i < model.FileUploadStatuses.Length; i++)
                {
                    CheckingFile.DeleteFile(model.FileUploadStatuses[i].Name, model.FileUploadStatuses[i].Typ, _webHostEnvironment);
                }
                _logger.LogError(ex.Message);
                throw;
            }
        }

        //pobieranie plikow
        public async Task<TaskModel[]> GetTaskModelsAsync(int? start, string? category, int? max, bool? banned, string? dateEnd, string? dateStart)
        {
            try
            {
                var json = await _fileData.GetTaskAsync(start, category, max, banned, dateEnd, dateStart);
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(json);

                return result.TaskModel.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }
    }
}
