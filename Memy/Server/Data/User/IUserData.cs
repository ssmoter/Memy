using Memy.Shared.Model;

namespace Memy.Server.Data.User
{
    public interface IUserData
    {
        Task<bool> EmailIsAvailable(string value);
        Task<string> GetEmail(string token);
        Task<UserPublicModel> GetProfil(string name);
        Task<bool> NameIsAvailable(string value);
        Task<T> RegisterUser<T>(string email, string name, string password);
        Task<T> RegisterUserConfirm<T>(string value);
        Task UpdateAvatar(string token, string avatar);
        Task UpdateName(string token, string value);
        Task UpdatePassword(string token, string old, string @new);
    }
}