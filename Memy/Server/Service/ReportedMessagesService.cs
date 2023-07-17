using Memy.Server.Data.User;
using Memy.Shared.Model;

using Microsoft.Extensions.Primitives;

namespace Memy.Server.Service
{
    public class ReportedMessagesService
    {
        private readonly ILogger _logger;
        private readonly IReportedMessagesData _messages;

        public ReportedMessagesService(ILogger logger, IReportedMessagesData messages)
        {
            _logger = logger;
            _messages = messages;
        }

        internal async Task<ReportedMessagesModel[]> GetMessages(string token)
        {
            try
            {
                var result = await _messages.GetMessages(token);

                return result.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
