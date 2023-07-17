using Memy.Server.Data.Reported;
using Memy.Shared.Model;

namespace Memy.Server.Service
{
    public class ReportedService
    {
        private readonly IReportedDataBase _reportedData;
        private readonly ILogger _logger;

        public ReportedService(IReportedDataBase reportedData, ILogger logger)
        {
            _reportedData = reportedData;
            _logger = logger;
        }

        public async Task<ReportedModel> SetReactionToFile(ReportedModel reported, string token)
        {
            try
            {
                var result = await _reportedData.SetReportedFile(reported.Id, reported.Value, token);
                reported.Value = result; 

                return reported;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

    }
}
