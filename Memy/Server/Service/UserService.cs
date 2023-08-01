using Memy.Server.Data;
using Memy.Server.Data.File;
using Memy.Server.Data.User;
using Memy.Shared.Helper;
using Memy.Shared.Model;

namespace Memy.Server.Service
{
    internal class UserService
    {
        private readonly IUserData _userData;

        private readonly IWebHostEnvironment _webHostEnvironment;

        internal UserService(IUserData userData, IWebHostEnvironment webHostEnvironment)
        {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                throw;
            }
        }

        internal async Task UpdatePassword(string? token, string? value)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(value))
                {
                    var password = ConvertByteString.ConvertToObject<Password>(value);
                    await _userData.UpdatePassword(token, password.Old, password.New);
                }
            }
            catch (Exception )
            {
                throw;
            }
        }


        internal async Task<bool> UpdateName(string? token, string? value)
        {
            try
            {
                bool available = false;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    available = await _userData.NameIsAvailable(value);
                }
                if (available)
                {
                    if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(value))
                    {
                        await _userData.UpdateName(token, value);
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal async Task UpdateAvatar(string? token, string? value)
        {
            FileUploadStatus? avatar = null;

            if (!string.IsNullOrWhiteSpace(value))
            {
                avatar = Newtonsoft.Json.JsonConvert.DeserializeObject<FileUploadStatus>(value);
            }


            try
            {

                if (avatar is null)
                {
                    throw new ArgumentNullException();
                }
                var status = await CheckingFile.CorrectData(avatar, _webHostEnvironment);

                if (status is not null)
                {
                    if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(status.ObjName))
                    {
                        await _userData.UpdateAvatar(token, status.ObjName);
                    }
                }
            }
            catch (Exception )
            {
                if (avatar is not null)
                {
                    CheckingFile.DeleteFile(avatar.ObjName, _webHostEnvironment);
                }
                throw;
            }
        }

        internal async Task<string> UpdateEmail(string? token, string? value)
        {
            await Task.Run(() => { Task.Delay(1); });
            throw new NotImplementedException();
        }
    }
}
