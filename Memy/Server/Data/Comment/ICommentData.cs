using Memy.Shared.Model;

namespace Memy.Server.Data.Comment
{
    public interface ICommentData
    {
        Task<T[]> GetComment<T>(string procedure, int id, int orderTyp, string token);
        Task<T[]> InsertComment<T>(string procedure, string token, string json, int orderTyp);
    }
}