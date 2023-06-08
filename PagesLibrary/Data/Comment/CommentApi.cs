using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;

using System.Text;

namespace PagesLibrary.Data.Comment
{
    internal class CommentApi : BaseApi, ICommentApi
    {
        public CommentApi(ILocalStorageService localStorageService,
                          ISessionStorageService sessionStorageService) : base(localStorageService, sessionStorageService)
        {
        }

        public async Task<HttpResponseMessage> GetCommentAsync(int id, int orderTyp = 0)
        {
            try
            {
                var client = await SetAuthorizationHeader();
                StringBuilder sb = new StringBuilder();
                sb.Append(Routes.Comment);
                sb.Append("/");
                sb.Append(id);
                sb.Append("?orderTyp=");
                sb.Append(orderTyp);

                var result = await client.GetAsync(sb.ToString());

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
