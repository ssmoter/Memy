using Memy.Server.Data.SqlDataAccess;
using Memy.Shared.Model;

using Newtonsoft.Json.Linq;

using System.Security.Cryptography;
using System.Xml.Linq;

namespace Memy.Server.Data.User
{
    public class UserData : BaseData, IUserData
    {
        public UserData(ISqlDataAccess sqlData) : base(sqlData)
        {
        }

        public async Task<string> GetEmail(string token)
        {
            sql.Clear();
            sql.Append("EXEC dbo.GetEmail");
            sql.Append("'");
            sql.Append(token);
            sql.Append("'");

            return await sqlData.LoadData<string>(sql.ToString());
        }

        public async Task<UserPublicModel> GetProfil(string name)
        {
            sql.Clear();
            sql.Append("EXEC dbo.GetProfile");
            sql.Append("'");
            sql.Append(name);
            sql.Append("'");

            return await sqlData.LoadData<UserPublicModel>(sql.ToString());
        }

        public async Task<bool> NameIsAvailable(string value)
        {
            var result = await ExecProcedure<bool>("NameIsAvailable", value);
            return result;
        }

        public async Task UpdateName(string token, string value)
        {
            await ExecProcedure<bool>("UpdateProfileName", value, token);
        }

        public async Task UpdatePassword(string token, string old, string @new)
        {
            await ExecProcedure<bool>("UpdateProfilePassword", old, @new, token);
        }
        public async Task UpdateAvatar(string token, string avatar)
        {
            await ExecProcedure<bool>("UpdateProfileAvatar", avatar, token);
        }

    }
}
