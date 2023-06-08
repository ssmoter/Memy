namespace PagesLibrary.Data.Comment
{
    internal interface ICommentApi
    {
        Task<HttpResponseMessage> GetCommentAsync(int id, int orderTyp = 0);
    }
}