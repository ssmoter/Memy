using Memy.Server.Data.SqlDataAccess;

namespace Memy.Server.Data.File
{
    public interface IAddNewFileModel
    {
        Task<bool> AddFileData(int FileSimpleId, string ImgName, string ImgType);
        Task<int> CreateNewFile(string token, string title, string description);
    }

    public class AddNewFileModel : BaseData, IAddNewFileModel
    {
        public AddNewFileModel(ISqlDataAccess sqlData) : base(sqlData)
        {
        }

        public async Task<int> CreateNewFile(string token, string title, string description)
        {
            sql.Clear();
            sql.Append("EXEC [CreateNewFile] ");
            sql.Append("N'");
            sql.Append(token);
            sql.Append("' ");
            sql.Append("N'");
            sql.Append(title);
            sql.Append("' ");
            sql.Append("N'");
            sql.Append(description);
            sql.Append("' ");

            return await sqlData.LoadData<int>(sql.ToString());
        }
        public async Task<bool> AddFileData(int FileSimpleId, string ImgName, string ImgType)
        {
            sql.Clear();
            sql.Append("EXEC [CreateNewFile] ");
            sql.Append("N'");
            sql.Append(FileSimpleId);
            sql.Append("' ");
            sql.Append("N'");
            sql.Append(ImgName);
            sql.Append("' ");
            sql.Append("N'");
            sql.Append(ImgType);
            sql.Append("' ");

            return await sqlData.LoadData<bool>(sql.ToString());
        }


    }
}
