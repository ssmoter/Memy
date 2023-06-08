namespace Memy.Server.Data.Comment
{
    public interface ICommentData
    {
        Task<string> GetComment(string procedure, int id, int orderTyp, string token);
        Task<string> InsertComment(string procedure, string token, string json, int orderTyp);
    }
}