using Memy.Server.Data.SqlDataAccess;
using Memy.Shared.Model;

namespace Memy.Server.Data.User
{
    public class ReportedMessagesData : BaseData, IReportedMessagesData
    {
        public ReportedMessagesData(ISqlDataAccess sqlData) : base(sqlData)
        {
        }


        public async Task<IList<ReportedMessagesModel>> GetMessages(string token)
        {
            var result = await ExecProcedureList<ReportedMessagesModel>("GetUserReportedMessages", token);
            return result;
        }

        public async Task<IList<ReportedMessagesModel>> UpdateMessages(string token, int id, bool beenChecked, bool beenDelete)
        {
            var result = await ExecProcedureList<ReportedMessagesModel>("UpdateUserReportedMessages", token,id,beenChecked,beenDelete);
            return result;
        }
    }
}
