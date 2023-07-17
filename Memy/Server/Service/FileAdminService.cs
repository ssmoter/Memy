using Memy.Server.Data.Admin;
using Memy.Server.Data.File;
using Memy.Shared.Model;

using Microsoft.Extensions.Primitives;

using PagesLibrary.Data;

namespace Memy.Server.Service
{
    public class FileAdminService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger _logger;
        private readonly IAdminData _adminData;

        public FileAdminService(IWebHostEnvironment webHostEnvironment, ILogger logger, IAdminData adminData)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _adminData = adminData;
        }

        public async Task DeleteFile(int id, string token, ReportedMessagesModel reported)
        {
            try
            {
                var result = await _adminData.DeleteFileByAdmin<string>(id, token, reported.Header, reported.Body, reported.Level);

                for (int i = 0; i < result.Length; i++)
                {
                    CheckingFile.DeleteFile(result[i], _webHostEnvironment);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        internal async Task BanFile(int id, string token, ReportedMessagesModel reported)
        {
            try
            {
                await _adminData.BanFileByAdmin(id, token, reported.Header, reported.Body, reported.Level);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        internal async Task CategoryFile(int id, string token, ReportedMessagesModel reportedMessagesModel)
        {
            try
            {
                await _adminData.BanFileByAdmin(id, token, reportedMessagesModel.Header, reportedMessagesModel.Body, reportedMessagesModel.Level);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
