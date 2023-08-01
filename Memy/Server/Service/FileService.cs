using Memy.Server.Data.File;
using Memy.Shared.Helper;
using Memy.Shared.Model;

namespace Memy.Server.Service
{
    public class FileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAddNewFileModel _fileData;

        public FileService(IWebHostEnvironment webHostEnvironment, IAddNewFileModel fileData)
        {
            _webHostEnvironment = webHostEnvironment;
            _fileData = fileData;
        }

        //Zapisywanie plików na dysku
        public async Task<FileUploadStatus[]?> SaveFile(FileUploadStatus[] model)
        {
            try
            {
                if (model is not null)
                {
                    var status = new FileUploadStatus();
                    for (int i = 0; i < model.Length; i++)
                    {
                        if (model[i].ObjTyp == (int)MyEnums.FileTyp.image || model[i].ObjTyp == (int)MyEnums.FileTyp.video)
                        {
                            if (i >= Memy.Shared.Helper.FileRequirements.MaxNumberOfFiles)
                                break;
                            status = await CheckingFile.CorrectData(model[i], _webHostEnvironment);
                            if (status is not null)
                            {
                                model[i] = status;
                            }
                        }
                    }
                }
                return model;
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                if (model is not null)
                {
                    if (model.FileUploadStatuses is not null)
                    {
                        for (int i = 0; i < model.FileUploadStatuses.Length; i++)
                        {
                            CheckingFile.DeleteFile(model.FileUploadStatuses[i].ObjName, _webHostEnvironment);
                        }
                    }
                }
                throw;
            }
        }

        //pobieranie plikow
        public async Task<TaskModel[]?> GetTaskModelsAsync(int? start, string? category, int? max, bool? banned, string? dateEnd, string? dateStart, int orderTyp, string? token)
        {
            try
            {
                var task = await _fileData.GetTasksAsync<GetTask>(start, category, max, banned, dateEnd, dateStart, orderTyp, token);

                var result = new TaskModel[task.Length];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = GetTask.ParseToTask(task[i]);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<TaskModel?> GetTaskModelAsync(int id, string? token)
        {
            try
            {
                var task = await _fileData.GetTaskAsync<GetTask>(id, token);

                var result = new TaskModel();

                result = GetTask.ParseToTask(task);


                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TaskModel[]?> GetUserTasksModel(string name, int start, int max, int orderTyp, bool banned)
        {
            try
            {
                var task = await _fileData.GetUserTasksModel<GetTask>(name, start, max, orderTyp, banned);

                var result = new TaskModel[task.Length];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = GetTask.ParseToTask(task[i]);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<TaskModel[]?> GetLikeUserTasksModel(string token, int start, int max, int orderTyp)
        {
            try
            {
                var task = await _fileData.GetLikeUserTasksModel<GetTask>(token, start, max, orderTyp);

                var result = new TaskModel[task.Length];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = GetTask.ParseToTask(task[i]);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private class GetTask
        {
            public int Id { get; set; }
            public bool Banned { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public DateTimeOffset CreatedDate { get; set; }
            public string? User { get; set; }
            public string? FileModel { get; set; }
            public string? Tag { get; set; }
            public string? Reaction { get; set; }
            public string? Reported { get; set; }

            public static T? GetValue<T>(string? value)
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

            public static TaskModel ParseToTask(GetTask task)
            {
                var result = new TaskModel();
                result.Id = task.Id;
                result.Title = task.Title;
                result.Description = task.Description;
                result.CreatedDate = task.CreatedDate;
                result.Banned = task.Banned;

                result.User = GetTask.GetValue<User?>(task.User);
                result.FileModel = GetTask.GetValue<FileModel[]?>(task.FileModel);
                result.Tag = GetTask.GetValue<Tag[]?>(task.Tag);
                result.Reaction = GetTask.GetValue<ReactionModel?>(task.Reaction);
                result.Reported = GetTask.GetValue<ReportedModel?>(task.Reported);
                return result;
            }

        }

    }
}
