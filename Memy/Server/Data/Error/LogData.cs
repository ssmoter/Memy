using Memy.Server.Data.SqlDataAccess;

namespace Memy.Server.Data.Error
{
    public class LogData : BaseData, ILogData
    {
        public LogData(ISqlDataAccess sqlData) : base(sqlData)
        {
        }

        public async Task SaveLog(string track,string message)
        {
            var a = await ExecProcedure<bool>("InsertError", track, message);
        }


    }
}
