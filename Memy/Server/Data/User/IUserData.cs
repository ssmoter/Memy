namespace Memy.Server.Data.User
{
    public interface IUserData
    {
        Task<bool> CheckToken(Guid? value);
        Task<T> LogIn<T>(string email, string password, bool doNotLogOut);
        Task LogOut(string value);
    }
}
