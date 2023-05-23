using Memy.Server.Data.SqlDataAccess;

using Newtonsoft.Json.Linq;

namespace Memy.Server.Data.File
{
    public interface IAddNewFileModel
    {
        Task<bool> AddFileData(int FileSimpleId, string ImgName, string ImgType);
        Task<bool> AddFileTag(int FileSimpleId, string Value);
        Task<int> CreateNewFile(string token, string title, string description);
        Task<string> GetTaskAsync(int? start, string? mcategoryain, int? max, bool? banned, string? dateEnd, string? dateStart);
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
        public async Task<int> CreateNewFile(string token, string title, string description)
        {
            sql.Clear();
            sql.Append("EXEC [CreateNewFile] ");
            sql.Append("N'");
            sql.Append(token);
            sql.Append("', ");
            sql.Append("N'");
            sql.Append(title);
            sql.Append("' ");
            if (!string.IsNullOrWhiteSpace(description))
            {
                sql.Append(",N'");
                sql.Append(description);
                sql.Append("'");
            }

            return await sqlData.LoadData<int>(sql.ToString());
        }
        public async Task<bool> AddFileData(int FileSimpleId, string ImgName, string ImgType)
        {
            sql.Clear();
            sql.Append("EXEC [AddFileData] ");
            sql.Append("");
            sql.Append(FileSimpleId);
            sql.Append(", ");
            sql.Append("N'");
            sql.Append(ImgName);
            sql.Append("', ");
            sql.Append("N'");
            sql.Append(ImgType);
            sql.Append("' ");

            return await sqlData.LoadData<bool>(sql.ToString());
        }
        public async Task<bool> AddFileTag(int FileSimpleId, string Value)
        {
            sql.Clear();
            sql.Append("EXEC [AddTag] ");
            sql.Append("");
            sql.Append(FileSimpleId);
            sql.Append(", ");
            sql.Append("N'");
            sql.Append(Value);
            sql.Append("'");

            return await sqlData.LoadData<bool>(sql.ToString());
        }
        public async Task<string> GetTaskAsync(int? start, string? category, int? max, bool? banned, string? dateEnd, string? dateStart)
        {
            sql.Clear();
            sql.Append("EXEC [GetFileByDate] ");
            if (start != null)
            {
                sql.Append(start);
            }
            if (max != null)
            {
                sql.Append(", ");
                sql.Append(max);
            }
            if (category != null)
            {
                sql.Append(", ");
                sql.Append(category);
            }
            if (banned!=null)
            {
                sql.Append(", ");
                sql.Append(banned);
            }
            if (dateEnd != null)
            {
                sql.Append(", ");
                sql.Append(dateEnd);
            }
            if (dateStart != null)
            {
                sql.Append(", ");
                sql.Append(dateStart);
            }

            return await sqlData.LoadData<string>(sql.ToString());
        }

    }
}
