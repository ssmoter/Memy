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
                if (model.FileUploadStatuses != null)
                {
                    for (int i = 0; i < model.FileUploadStatuses.Length; i++)
                    {
                        CheckingFile.DeleteFile(model.FileUploadStatuses[i].Name, model.FileUploadStatuses[i].Typ, _webHostEnvironment);
                    }
                }
                _logger.LogError(ex.Message);
                throw;
            }
        }

        //pobieranie plikow
        public async Task<TaskModel[]?> GetTaskModelsAsync(int? start, string? category, int? max, bool? banned, string? dateEnd, string? dateStart, string? token)
        {
            try
            {
                var task = await _fileData.GetTaskAsync<GetTask>(start, category, max, banned, dateEnd, dateStart, token);

                var result = new TaskModel[task.Length];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new TaskModel();
                    result[i].Id = task[i].Id;
                    result[i].Title = task[i].Title;
                    result[i].Description = task[i].Description;
                    result[i].CreatedDate = task[i].CreatedDate;

                    result[i].User = GetTask.GetValue<User?>(task[i].User);
                    result[i].FileModel = GetTask.GetValue<FileModel[]?>(task[i].FileModel);
                    result[i].Tag = GetTask.GetValue<Tag[]?>(task[i].Tag);
                    result[i].Reaction = GetTask.GetValue<ReactionModel?>(task[i].Reaction);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }
        private class GetTask
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public DateTimeOffset CreatedDate { get; set; }
            public string? User { get; set; }
            public string? FileModel { get; set; }
            public string? Tag { get; set; }
            public string? Reaction { get; set; }

            public static T? GetValue<T>(string value)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
                }
                else
                {
                    return default(T?);
                }
            }
        }

    }
}
