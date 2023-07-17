using Memy.Server.Data.SqlDataAccess;
using Memy.Shared.Model;

namespace Memy.Server.Data.File
{
    public interface IAddNewFileModel
    {
        Task<T[]> GetLikeUserTasksModel<T>(string token, int start, int max, int orderTyp);
        Task<IList<string>> GetTagList();
        Task<T> GetTaskAsync<T>(int id, string? token);
        Task<T[]> GetTasksAsync<T>(int? start, string? mcategoryain, int? max, bool? banned, string? dateEnd, string? dateStart, int orderTyp, string? token);
        Task<T[]> GetUserTasksModel<T>(string name, int start, int max, int orderTyp, bool banned);
        Task<int> InsertFullFile(string json, string token);
    }

    public class AddNewFileModel : BaseData, IAddNewFileModel
    {
        public AddNewFileModel(ISqlDataAccess sqlData) : base(sqlData)
        {
        }

        public async Task<int> InsertFullFile(string json, string token)
        {
            sql.Clear();
            sql.Append("EXEC [InsertFullFile] ");
            sql.Append("N'");
            sql.Append(json);
            sql.Append("', ");
            sql.Append("N'");
            sql.Append(token);
            sql.Append("'");

            return await sqlData.LoadData<int>(sql.ToString());
        }
        public async Task<T[]> GetTasksAsync<T>(int? start, string? category, int? max, bool? banned, string? dateEnd, string? dateStart, int orderTyp, string? token)
        {
            sql.Clear();
            sql.Append("EXEC [GetFiles] ");

            sql.Append(start);

            sql.Append(", ");
            sql.Append(max);

            sql.Append(", '");
            sql.Append(category);
            sql.Append("'");

            sql.Append(", ");
            sql.Append(banned);

            sql.Append(", '");
            sql.Append(dateEnd);
            sql.Append("'");

            sql.Append(", '");
            sql.Append(dateStart);
            sql.Append("'");


            sql.Append(", ");
            sql.Append(orderTyp);

            if (token != null)
            {
                sql.Append(", '");
                sql.Append(token);
                sql.Append("'");
            }

            return (await sqlData.LoadDataList<T>(sql.ToString())).ToArray();
        }
        public async Task<T> GetTaskAsync<T>(int id, string? token)
        {
            sql.Clear();
            sql.Append("EXEC [GetSingleFile] ");
            sql.Append(id);

            if (token != null)
            {
                sql.Append(", '");
                sql.Append(token);
                sql.Append("'");
            }

            return await sqlData.LoadData<T>(sql.ToString());
        }

        public async Task<IList<string>> GetTagList()
        {
            sql.Clear();
            sql.Append("SELECT Value FROM [dbo].[FileTagList]");
            return await sqlData.LoadDataList<string>(sql.ToString());
        }

        public async Task<T[]> GetUserTasksModel<T>(string name, int start, int max, int orderTyp, bool banned)
        {
            var result = await this.ExecProcedureList<T>("GetUserFiles", name, start, max, orderTyp, banned);
            return result.ToArray();
        }
        public async Task<T[]> GetLikeUserTasksModel<T>(string token, int start, int max, int orderTyp )
        {
            var result = await this.ExecProcedureList<T>("GetUserLikeFiles", token, start, max, orderTyp);
            return result.ToArray();
        }
    }
}
