using Memy.Server.Data.SqlDataAccess;

using System.Text;

namespace Memy.Server.Data
{
    public class BaseData
    {
        public readonly ISqlDataAccess sqlData;
        public StringBuilder sql;
        public BaseData(ISqlDataAccess sqlData)
        {
            this.sqlData = sqlData;
            this.sql = new StringBuilder();
        }

        public async Task<T> ExecProcedure<T>(string procedure, params object?[] args)
        {
            sql.Clear();
            sql.Append("EXEC [dbo].[");
            sql.Append(procedure);
            sql.Append("] ");

            for (int i = 0; i < args.Length; i++)
            {
                if (i > 0)
                {
                    sql.Append(", ");
                }
                if (args[i] is string)
                {
                    sql.Append("N'");
                    sql.Append(args[i]);
                    sql.Append("' ");
                }
                else
                {
                    sql.Append(args[i]);
                }
            }
            return await sqlData.LoadData<T>(sql.ToString());
        }
        public async Task<IList<T>> ExecProcedureList<T>(string procedure, params object?[] args)
        {
            sql.Clear();
            sql.Append("EXEC [dbo].[");
            sql.Append(procedure);
            sql.Append("] ");

            for (int i = 0; i < args.Length; i++)
            {
                if (i > 0)
                {
                    sql.Append(", ");
                }
                if (args[i] is string)
                {
                    sql.Append("N'");
                    sql.Append(args[i]);
                    sql.Append("' ");
                }
                else
                {
                    sql.Append(args[i]);
                }
            }
            return await sqlData.LoadDataList<T>(sql.ToString());
        }
    }
}
