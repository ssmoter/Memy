using Memy.Shared.Model;

namespace Memy.Server.Data.User
{
    public interface IReportedMessagesData
    {
        Task<IList<ReportedMessagesModel>> GetMessages(string token);
        Task<IList<ReportedMessagesModel>> UpdateMessages(string token, int id, bool beenChecked, bool beenDelete);
    }
}