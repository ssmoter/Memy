using Memy.Server.Data.SqlDataAccess;

namespace Memy.Server.Data.Admin
{
    public class AdminData : BaseData, IAdminData
    {
        public AdminData(ISqlDataAccess sqlData) : base(sqlData)
        {
        }

        public async Task BanFileByAdmin(int id, string token, string header, string body, int level)
        {
            await ExecProcedure<bool>("BanFileAsAdmin", id, token, header, body, level);
        }

        public async Task<T[]> DeleteFileByAdmin<T>(int id, string token, string header, string body, int level)
        {
            var result = await ExecProcedureList<T>("DeleteFileAsAdmin", id, token, header, body, level);
            return result.ToArray();
        }

        public async Task UpdateCategoryFileByAdmin(int id, string category, string token, string? header, string? body, int level)
        {
            await ExecProcedure<bool>("UpdateCategoryFileByAdmin", id, category, token, header, body, level);
        }
    }
}