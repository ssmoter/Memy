using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Components.Authorization;

using System.Net.Http.Json;

namespace PagesLibrary.Data.Admin
{
    public class AdminCommentApi : BaseApi, IAdminApi
    {
        public AdminCommentApi(ILocalStorageService localStorageService, ISessionStorageService sessionStorageService, AuthenticationStateProvider authenticationStateProvider = null) : base(localStorageService, sessionStorageService, authenticationStateProvider)
        {
        }

        public async Task<HttpResponseMessage> Delete(int id, ReportedMessagesModel reported)
        {
            try
            {
                var client = await this.SetAuthorizationHeader();
                var result = await client.PutAsJsonAsync($"{Routes.CommentAdmin}/{id}/{Routes.Delete}", reported);
                await IfUnauthorized(result);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> Ban(int id, ReportedMessagesModel reported)
        {
            try
            {
                var client = await this.SetAuthorizationHeader();


                var result = await client.PutAsJsonAsync($"{Routes.CommentAdmin}/{id}/{Routes.Ban}", reported);
                await IfUnauthorized(result);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <param name="reported"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<HttpResponseMessage> UpdateCategory(int id, string category, ReportedMessagesModel reported)
        {
            throw new NotImplementedException();
        }
    }
}
