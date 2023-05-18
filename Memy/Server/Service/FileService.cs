using Memy.Server.Data.File;
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

        public async Task InsertIntoDataBase(FileUploadModel model, string token)
        {
            try
            {
                var id = await _fileData.CreateNewFile(token, model.Title, model.Description);
                for (int i = 0; i < model.FileUploadStatuses.Length; i++)
                {
                    await _fileData.AddFileData(id, model.FileUploadStatuses[i].Name, model.FileUploadStatuses[i].Typ);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

    }
}
