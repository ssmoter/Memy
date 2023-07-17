using Memy.Server.Data.File;
using Memy.Server.Data.User;
using Memy.Shared.Helper;
using Memy.Shared.Model;

namespace Memy.Server.Service
{
    internal class UserService
    {
        private readonly ILogger _logger;
        private readonly IUserData _userData;

        private readonly IWebHostEnvironment _webHostEnvironment;

        internal UserService(ILogger logger, IUserData userData, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _userData = userData;
            _webHostEnvironment = webHostEnvironment;
        }

        internal async Task<UserPublicModel> GetProfile(string username)
        {
            try
            {
                var result = await _userData.GetProfil(username);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        internal async Task<string> GetEmail(string token)
        {
            try
            {
                var result = await _userData.GetEmail(token);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        internal async Task UpdatePassword(string token, string value)
        {
            try
            {
                var password = ConvertByteString.ConvertToObject<Password>(value);
                await _userData.UpdatePassword(token, password.Old, password.New);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


        internal async Task<bool> UpdateName(string token, string value)
        {
            try
            {
                var available = await _userData.NameIsAvailable(value);
                if (available)
                {
                    await _userData.UpdateName(token, value);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        internal async Task UpdateAvatar(string token, string value)
        {
            var avatar = Newtonsoft.Json.JsonConvert.DeserializeObject<FileUploadStatus>(value);
            try
            {

                if (avatar is null)
                {
                    throw new ArgumentNullException();
                }
                var status = await CheckingFile.CorrectData(avatar, _webHostEnvironment);

                await _userData.UpdateAvatar(token, status.ObjName);
            }
            catch (Exception ex)
            {
                CheckingFile.DeleteFile(avatar.ObjName, _webHostEnvironment);
                _logger.LogError(ex.Message);
                throw;
            }
        }

        internal async Task<string> UpdateEmail(string token, string value)
        {
            throw new NotImplementedException();
        }
    }
}
