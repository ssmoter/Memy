using Memy.Server.Data.SqlDataAccess;

namespace Memy.Server.Data.Reported
{
    public class ReportedDataBase : BaseData, IReportedDataBase
    {
        public ReportedDataBase(ISqlDataAccess sqlData) : base(sqlData)
        {
        }



        public async Task<int> SetReportedFile(int fileSimpleId, int value, string token)
        {
            var result = await ExecProcedure<int>("InsertReportFile", fileSimpleId, value, token);
            return result;
        }

    }
}
