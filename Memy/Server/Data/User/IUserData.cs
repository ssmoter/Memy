using Memy.Shared.Model;

namespace Memy.Server.Data.User
{
    public interface IUserData
    {
        Task<string> GetEmail(string token);
        Task<UserPublicModel> GetProfil(string name);
        Task<bool> NameIsAvailable(string value);
        Task UpdateAvatar(string token, string avatar);
        Task UpdateName(string token, string value);
        Task UpdatePassword(string token, string old, string @new);
    }
}