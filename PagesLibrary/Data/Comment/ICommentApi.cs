namespace PagesLibrary.Data.Comment
{
    internal interface ICommentApi
    {
        Task<HttpResponseMessage> GetAnswerCommentAsync(int id, int orderTyp = 0);
        Task<HttpResponseMessage> GetCommentAsync(int id, int orderTyp = 0);
        Task<int> GetOrderTyp();
        Task<HttpResponseMessage> GetUserCommentAsync(string? name, int? orderTyp);
        Task<HttpResponseMessage> SendAnswerCommentAsync(Memy.Shared.Model.Comment comment, int orderTyp = 0);
        Task<HttpResponseMessage> SendCommentAsync(Memy.Shared.Model.Comment comment, int orderTyp = 0);
        Task SetOrderTyp(int order);
    }
}