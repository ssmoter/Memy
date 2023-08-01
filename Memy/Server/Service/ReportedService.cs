using Memy.Server.Data;
using Memy.Server.Data.Reported;
using Memy.Shared.Model;

namespace Memy.Server.Service
{
    public class ReportedService
    {
        private readonly IReportedDataBase _reportedData;

        public ReportedService(IReportedDataBase reportedData)
        {
            _reportedData = reportedData;
        }

        public async Task<ReportedModel> SetReactionToFile(ReportedModel reported, string token)
        {
            try
            {
                var result = await _reportedData.SetReportedFile(reported.Id, reported.Value, token);
                reported.Value = result; 

                return reported;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
