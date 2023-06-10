using Memy.Server.Data.SqlDataAccess;
using Memy.Shared.Model;

namespace Memy.Server.Data.Comment
{
    public class CommentData : BaseData, ICommentData
    {
        public CommentData(ISqlDataAccess sqlData) : base(sqlData)
        {
        }

        public async Task<T[]> InsertComment<T>(string procedure, string token, string json,int orderTyp)
        {
            sql.Clear();
            sql.Append("EXEC [dbo].");
            sql.Append(procedure);
            sql.Append(" '");
            sql.Append(token);
            sql.Append("','");
            sql.Append(json);
            sql.Append("', ");
            sql.Append(orderTyp);

            return (await sqlData.LoadDataList<T>(sql.ToString())).ToArray();
        }

        public async Task<T[]> GetComment<T>(string procedure, int id, int orderTyp, string token)
        {
            sql.Clear();
            sql.Append("EXEC [dbo].");
            sql.Append(procedure);
            sql.Append(' ');
            sql.Append(id);
            sql.Append(", ");
            sql.Append(orderTyp);
            sql.Append(", '");
            sql.Append(token);
            sql.Append("'");

            return (await sqlData.LoadDataList<T>(sql.ToString())).ToArray();
        }

    }
}
