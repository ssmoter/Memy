using Memy.Server.Data.SqlDataAccess;

namespace Memy.Server.Data.User
{
    public class LoginData : BaseData, ILoginData
    {
        public LoginData(ISqlDataAccess sqlData) : base(sqlData)
        {

        }
        public async Task<bool> CheckAdminToken(Guid? value)
        {
            sql.Clear();
            sql.Append("EXEC CheckAdminToken ");
            sql.Append("'");
            sql.Append(value.ToString());
            sql.Append("'");

            return await sqlData.LoadData<bool>(sql.ToString());
        }

        public async Task<bool> CheckToken(Guid? value)
        {
            sql.Clear();
            sql.Append("EXEC CheckToken ");
            sql.Append("'");
            sql.Append(value.ToString());
            sql.Append("'");

            return await sqlData.LoadData<bool>(sql.ToString());
        }

        public async Task LogOut(string value)
        {
            sql.Clear();
            sql.Append("EXEC LogoutUser ");
            sql.Append("N'");
            sql.Append(value);
            sql.Append("'");

            await sqlData.SaveData(sql.ToString(),new bool());
        }

        public async Task<T> LogIn<T>(string email,string password,bool doNotLogOut)
        {
            sql.Clear();
            sql.Append("EXEC LoginUser ");
            sql.Append("N'");
            sql.Append(email);
            sql.Append("', N'");
            sql.Append(password);
            sql.Append("'");
            if (doNotLogOut)
            {
                sql.Append(", '");
                sql.Append("1");
                sql.Append("'");
            }
            return await sqlData.LoadData<T>(sql.ToString());
        }

    }
}
