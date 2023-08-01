using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Components.Authorization;

using PagesLibrary.Helper;

using System.Net;
using System.Net.Http.Json;

namespace PagesLibrary.Data.User
{
    public interface ILogInOut
    {
        Task<(string, HttpStatusCode)> LogIn(string? email, string? password, bool doNotLogOut);
        Task<(string, HttpStatusCode)> LogOut(Token token);
    }

    public class LogInOut : BaseApi, ILogInOut
    {
        public LogInOut(HttpClient httpClient, ILocalStorageService localStorageService, ISessionStorageService sessionStorageService, AuthenticationStateProvider authenticationStateProvider) : base(httpClient, localStorageService, sessionStorageService, authenticationStateProvider)
        {
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
                    RodoAvailable.AcceptedCookieNotice = true;
                    await SetAcceptedCookie();
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
                    ArgumentNullException.ThrowIfNull(userStorage);
                    ArgumentNullException.ThrowIfNullOrEmpty(userStorage.Token);
                    token.Value = Guid.Parse(userStorage.Token);
                }

                var requestToken = token.Value.ToString();
                ArgumentNullException.ThrowIfNullOrEmpty(requestToken);
                var url = $"{Routes.UserLog}/{requestToken.ToUpper()}";

                var result = await _httpClient.DeleteAsync(url);

                var content = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    if (userStorage is not null)
                    {
                        if (!string.IsNullOrWhiteSpace(userStorage.Token))
                        {
                           var strToken = token.Value.ToString();
                            if (!string.IsNullOrWhiteSpace(strToken))
                            {
                                if (userStorage.Token.ToUpper() == strToken.ToUpper())
                                {
                                    //usunać do local storage
                                    await GetLocalStorage().RemoveItemAsync(Headers.Authorization);
                                    //usunąć do seasion storage
                                    await GetSessionStorage().RemoveItemAsync(Headers.Authorization);
                                }
                            }
                        }
                    }
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
