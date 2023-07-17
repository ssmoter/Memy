using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace PagesLibrary.Data.User
{
    public interface ILogInOut
    {
        Task<(string, HttpStatusCode)> LogIn(string? email, string? password, bool doNotLogOut);
        Task<(string, HttpStatusCode)> LogOut(Token token);
    }

    public class LogInOut : BaseApi, ILogInOut
    {
        public LogInOut(ILocalStorageService localStorageService, ISessionStorageService sessionStorageService) : base(localStorageService, sessionStorageService)
        {
        }

        public LogInOut(ILocalStorageService localStorageService, ISessionStorageService sessionStorageService, AuthenticationStateProvider authenticationStateProvider = null) : base(localStorageService, sessionStorageService, authenticationStateProvider)
        {
        }

        public async Task<(string, HttpStatusCode)> Register(string name,string email,string password)
        {
            try
            {
                var _httpClient = GetHttpClient();
                var newUser = new UserSimple()
                {
                };
                //przesłanie hasła w postaci byte zapisanych jako string
                newUser.Password = System.BitConverter.ToString(System.Text.Encoding.UTF8.GetBytes(password));
                var result = await _httpClient.PostAsJsonAsync(Routes.UserLog, newUser);
                var content = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    if (newUser.Token.DoNotLogOut)
                    {
                        //zapisać do local storage
                        await GetLocalStorage().SetItemAsStringAsync(Headers.Authorization, content);
                    }
                    else
                    {
                        //zapisać do seasion storage
                        await GetSessionStorage().SetItemAsStringAsync(Headers.Authorization, content);
                    }
                }
                return (content, result.StatusCode);
            }
            catch (Exception )
            {
                throw;
            }
        }

        //zalogowanie użytkownia
        public async Task<(string, HttpStatusCode)> LogIn(string? email, string? password, bool doNotLogOut)
        {
            try
            {

                var _httpClient = GetHttpClient();
                var newUser = new UserSimple()
                {
                    Email = email,
                    Token = new Token()
                    {
                        DoNotLogOut = doNotLogOut,
                    }
                };
                //przesłanie hasła w postaci byte zapisanych jako string
                newUser.Password = ConvertByteString.ConvertToString(password);
                var result = await _httpClient.PostAsJsonAsync(Routes.UserLog, newUser);
                var content = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    if (newUser.Token.DoNotLogOut)
                    {
                        //zapisać do local storage
                        await GetLocalStorage().SetItemAsStringAsync(Headers.Authorization, content);
                    }
                    else
                    {
                        //zapisać do seasion storage
                        await GetSessionStorage().SetItemAsStringAsync(Headers.Authorization, content);
                    }
                }
                return (content, result.StatusCode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //wylogowanie użytkownika
        public async Task<(string, HttpStatusCode)> LogOut(Token token)
        {
            try
            {
                var _httpClient = await base.SetAuthorizationHeader();
                UserStorage? userStorage = await GetUserStorage();

                if (token.Value == null)
                {
                    token.Value = Guid.Parse(userStorage.Token);
                }

                var result = await _httpClient.DeleteAsync($"{Routes.UserLog}/{token.Value.ToString().ToUpper()}");

                var content = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode
                    && userStorage.Token.ToUpper() == token.Value.ToString().ToUpper())
                {
                    //usunać do local storage
                    await GetLocalStorage().RemoveItemAsync(Headers.Authorization);
                    //usunąć do seasion storage
                    await GetSessionStorage().RemoveItemAsync(Headers.Authorization);
                }

                await IfUnauthorized(result);

                return (content, result.StatusCode);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
