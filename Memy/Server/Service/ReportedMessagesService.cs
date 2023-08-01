using Memy.Server.Data.User;
using Memy.Shared.Model;

namespace Memy.Server.Service
{
    public class ReportedMessagesService
    {
        private readonly IReportedMessagesData _messages;

        public ReportedMessagesService(IReportedMessagesData messages)
        {
            _messages = messages;
        }

        internal async Task<ReportedMessagesModel[]> GetMessages(string token)
        {
            try
            {
                var result = await _messages.GetMessages(token);

                return result.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal async Task<ReportedMessagesModel[]> UpdateMessages(string token, ReportedMessagesModel messagesModel)
        {
            try
            {
                var result = await _messages.UpdateMessages(token, messagesModel.Id, messagesModel.BeenChecked, messagesModel.BeenDelete);

                return result.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
