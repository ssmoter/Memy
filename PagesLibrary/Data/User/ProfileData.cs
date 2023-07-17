using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Components.Authorization;

namespace PagesLibrary.Data.User
{
    public class ProfileData : BaseApi, IProfileData
    {
        public ProfileData(HttpClient httpClient, ILocalStorageService localStorageService, ISessionStorageService sessionStorageService, AuthenticationStateProvider authenticationStateProvider) : base(httpClient, localStorageService, sessionStorageService, authenticationStateProvider)
        {
        }

        public async Task<HttpResponseMessage> GetProfileAsync(string name)
        {
            try
            {
                var client = GetHttpClient();
                var result = await client.GetAsync($"{Routes.User}/{name}");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<string> GetEmailAsync()
        {
            try
            {
                var client = await this.SetAuthorizationHeader();
                var result = await client.GetStringAsync($"{Routes.User}/{Routes.Email}");
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> UpdateProfil(Memy.Shared.Helper.MyEnums.UpdateProfile id, object value)
        {
            try
            {
                var client = await SetAuthorizationHeader();

                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(value), null, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Put, $"{Routes.User}/{(int)id}");
                request.Content = content;

                var result = await client.SendAsync(request);
                await IfUnauthorized(result);

                if (result.IsSuccessStatusCode && id == MyEnums.UpdateProfile.Name)
                {
                    await UpdateStorege((string)value);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }

            async Task UpdateStorege(string name)
            {
                var local = await this.GetLocalStorage().GetItemAsStringAsync(Memy.Shared.Helper.Headers.Authorization);
                if (local != null)
                {
                    var user = Memy.Shared.Helper.ConvertByteString.ConvertToObject<UserStorage>(local);
                    user.UserName = name;
                    await this.GetLocalStorage()
                        .SetItemAsStringAsync(Memy.Shared.Helper.Headers.Authorization
                        , Memy.Shared.Helper.ConvertByteString.ConvertToString(user));
                }
                var session = await this.GetSessionStorage().GetItemAsStringAsync(Memy.Shared.Helper.Headers.Authorization);
                if (session != null)
                {
                    var user = Memy.Shared.Helper.ConvertByteString.ConvertToObject<UserStorage>(local);
                    user.UserName = name;
                    await this.GetLocalStorage()
                        .SetItemAsStringAsync(Memy.Shared.Helper.Headers.Authorization,
                        Memy.Shared.Helper.ConvertByteString.ConvertToString(user));
                }
            }

        }

    }
}
