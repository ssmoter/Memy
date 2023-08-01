using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;

using Microsoft.AspNetCore.Components.Authorization;

using System.Net.Http.Json;
using System.Text;

namespace PagesLibrary.Data.Comment
{
    internal class CommentApi : BaseApi, ICommentApi
    {
        public CommentApi(HttpClient httpClient, ILocalStorageService localStorageService, ISessionStorageService sessionStorageService, AuthenticationStateProvider authenticationStateProvider) : base(httpClient, localStorageService, sessionStorageService, authenticationStateProvider)
        {
        }


        // pobieranie i wysyłanie komentarzy
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
                await IfUnauthorized(result);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<HttpResponseMessage> SendCommentAsync(Memy.Shared.Model.Comment comment, int orderTyp = 0)
        {
            try
            {
                var client = await SetAuthorizationHeader();
                string url = $"{Routes.Comment}?orderTyp={orderTyp}";

                var result = await client.PostAsJsonAsync(url, comment);
                await IfUnauthorized(result);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //pobiranie i wysyłanie odpowiedzi do komentarzy
        public async Task<HttpResponseMessage> GetAnswerCommentAsync(int id, int orderTyp = 0)
        {
            try
            {
                var client = await SetAuthorizationHeader();
                StringBuilder sb = new StringBuilder();
                sb.Append(Routes.Comment);
                sb.Append("/answer/");
                sb.Append(id);
                sb.Append("?orderTyp=");
                sb.Append(orderTyp);

                var result = await client.GetAsync(sb.ToString());
                await IfUnauthorized(result);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<HttpResponseMessage> SendAnswerCommentAsync(Memy.Shared.Model.Comment comment, int orderTyp = 0)
        {
            try
            {
                var client = await SetAuthorizationHeader();
                string url = $"{Routes.Comment}/answer?orderTyp={orderTyp}";

                var result = await client.PostAsJsonAsync(url, comment);
                await IfUnauthorized(result);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetOrderTyp()
        {
            var local = this.GetLocalStorage();
            int order = 0;
            if (local is not null)
            {
                order = await local.GetItemAsync<int>("order");
            }
            return order;
        }
        public async Task SetOrderTyp(int order)
        {
            var local = this.GetLocalStorage();
            if (local is not null)
            {
                await local.SetItemAsync<int>("order", order);
            }
        }

        public async Task<HttpResponseMessage> GetUserCommentAsync(string? name, int? orderTyp)
        {
            try
            {
                var client = await SetAuthorizationHeader();
                StringBuilder sb = new StringBuilder(Routes.Comment);

                sb.Append("/");
                sb.Append(Routes.User);

                if (name != null || orderTyp != null)
                {
                    sb.Append("?");
                }
                if (name != null)
                {
                    sb.Append("name=");
                    sb.Append(name);
                    sb.Append("&");
                }
                if (orderTyp != null)
                {
                    sb.Append("orderTyp=");
                    sb.Append(orderTyp);
                    sb.Append("&");
                }
                var result = await client.GetAsync(sb.ToString());
                await IfUnauthorized(result);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
