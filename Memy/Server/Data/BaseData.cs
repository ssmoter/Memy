using Memy.Server.Data.SqlDataAccess;
using Memy.Shared.Model;

using System.Net.Quic;
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

    }
}
