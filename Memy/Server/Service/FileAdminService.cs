using Memy.Server.Data.Admin;
using Memy.Server.Data.File;
using Memy.Shared.Model;

namespace Memy.Server.Service
{
    public class FileAdminService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAdminData _adminData;

        public FileAdminService(IWebHostEnvironment webHostEnvironment, IAdminData adminData)
        {
            _webHostEnvironment = webHostEnvironment;
            _adminData = adminData;
        }

        public async Task DeleteFile(int id, string? token, ReportedMessagesModel reported)
        {
            try
            {
                ArgumentNullException.ThrowIfNullOrEmpty(token);
                string head = "";
                string body = "";
                if (!string.IsNullOrWhiteSpace(reported.Header))
                {
                    head = reported.Header;
                }
                if (!string.IsNullOrWhiteSpace(reported.Body))
                {
                    body = reported.Body;
                }

                var result = await _adminData.DeleteFileByAdmin<string>(id, token, head, body, reported.Level);


                for (int i = 0; i < result.Length; i++)
                {
                    CheckingFile.DeleteFile(result[i], _webHostEnvironment);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal async Task BanFile(int id, string? token, ReportedMessagesModel reported)
        {
            try
            {
                ArgumentNullException.ThrowIfNullOrEmpty(token);
                string head = "";
                string body = "";
                if (!string.IsNullOrWhiteSpace(reported.Header))
                {
                    head = reported.Header;
                }
                if (!string.IsNullOrWhiteSpace(reported.Body))
                {
                    body = reported.Body;
                }
                await _adminData.BanFileByAdmin(id, token, head, body, reported.Level);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal async Task UpdateCategoryFile(int id, string category, string token, ReportedMessagesModel reportedMessagesModel)
        {
            try
            {
                await _adminData.UpdateCategoryFileByAdmin(id, category, token, reportedMessagesModel.Header, reportedMessagesModel.Body, reportedMessagesModel.Level);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
