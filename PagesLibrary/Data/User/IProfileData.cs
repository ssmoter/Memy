namespace PagesLibrary.Data.User
{
    public interface IProfileData
    {
        Task<string> GetEmailAsync();
        Task<HttpResponseMessage> GetProfileAsync(string name);
        Task<HttpResponseMessage> UpdateProfil(Memy.Shared.Helper.MyEnums.UpdateProfile id, object? value);
    }
}