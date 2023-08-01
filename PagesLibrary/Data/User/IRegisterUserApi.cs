using Memy.Shared.Model;

namespace PagesLibrary.Data.User
{
    public interface IRegisterUserApi
    {
        Task<HttpResponseMessage> PostRegister(string name, string email, string password);
        Task<HttpResponseMessage> PostRegisterConfirm(string value);
    }
}