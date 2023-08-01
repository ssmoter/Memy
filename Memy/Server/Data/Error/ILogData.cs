namespace Memy.Server.Data.Error
{
    public interface ILogData
    {
        Task SaveLog(string track, string message);
    }
}