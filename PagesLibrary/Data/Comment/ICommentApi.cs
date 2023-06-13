namespace PagesLibrary.Data.Comment
{
    internal interface ICommentApi
    {
        Task<HttpResponseMessage> GetAnswerCommentAsync(int id, int orderTyp = 0);
        Task<HttpResponseMessage> GetCommentAsync(int id, int orderTyp = 0);
        Task<HttpResponseMessage> SendAnswerCommentAsync(Memy.Shared.Model.Comment comment, int orderTyp = 0);
        Task<HttpResponseMessage> SendCommentAsync(Memy.Shared.Model.Comment comment, int orderTyp = 0);
    }
}