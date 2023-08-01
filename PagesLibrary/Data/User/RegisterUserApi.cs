using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Components.Authorization;

using System.Net.Http.Json;

namespace PagesLibrary.Data.User
{
    public class RegisterUserApi : BaseApi, IRegisterUserApi
    {
        public RegisterUserApi(HttpClient httpClient, ILocalStorageService localStorageService, ISessionStorageService sessionStorageService, AuthenticationStateProvider authenticationStateProvider) : base(httpClient, localStorageService, sessionStorageService, authenticationStateProvider)
        {

        }

        public async Task<HttpResponseMessage> PostRegister(string name, string email, string password)
        {
            try
            {
                var client = GetHttpClient();
                var value = new UserSimple()
                {
                    Nick = name,
                    Email = email,
                    Password = ConvertByteString.ConvertToString(password)
                };

                var result = await client.PostAsJsonAsync(Routes.User, value);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> PostRegisterConfirm(string value)
        {
            try
            {
                var client = GetHttpClient();

                var result = await client.GetAsync($"{Routes.User}/{value}");

                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    await GetSessionStorage().SetItemAsStringAsync(Headers.Authorization, content);
                }
                else
                {
                    throw new Exception(content);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
