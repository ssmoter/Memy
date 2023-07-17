namespace Memy.Server.Data.Reported
{
    public interface IReportedDataBase
    {
        Task<int> SetReportedFile(int fileSimpleId, int value, string token);
    }
}