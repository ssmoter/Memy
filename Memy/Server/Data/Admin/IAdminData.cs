namespace Memy.Server.Data.Admin
{
    public interface IAdminData
    {
        Task BanFileByAdmin(int id, string token, string header, string body, int level);
        Task<T[]> DeleteFileByAdmin<T>(int id, string token, string header, string body, int level);
        Task UpdateCategoryFileByAdmin(int id,string category, string token, string? header, string? body, int level);
    }
}