using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Components.Authorization;

using PagesLibrary.Helper;

using System.Net;

namespace PagesLibrary.Data
{
    public class BaseApi
    {
        private static HttpClient? _HttpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly ISessionStorageService _sessionStorageService;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public BaseApi(HttpClient httpClient,
                       ILocalStorageService localStorageService,
                       ISessionStorageService sessionStorageService,
                       AuthenticationStateProvider authenticationStateProvider)
        {
            _HttpClient = new HttpClient();
            if (httpClient is not null && httpClient.BaseAddress is not null)
            {
                _HttpClient.BaseAddress = new Uri(httpClient.BaseAddress, "/api/");
            }
            _localStorageService = localStorageService;
            _sessionStorageService = sessionStorageService;
            this.authenticationStateProvider = authenticationStateProvider;
        }
        public static HttpClient GetHttpClient()
        {
            if (_HttpClient is not null)
            {
                return _HttpClient;
            }
            return new HttpClient();
        }
        public ILocalStorageService GetLocalStorage()
        {
            if (RodoAvailable.AcceptedCookieNotice)
                return _localStorageService;
            else
                return null;
        }
        public ISessionStorageService GetSessionStorage()
        {
            if (RodoAvailable.AcceptedCookieNotice)
                return _sessionStorageService;
            else
                return null;
        }
        public async Task<HttpClient> SetAuthorizationHeader()
        {
            UserStorage? userStorage = await GetUserStorage();
            var client = GetHttpClient();
            client.DefaultRequestHeaders.Clear();
            if (userStorage is not null && !string.IsNullOrWhiteSpace(userStorage.Token))
            {
                client.DefaultRequestHeaders.Add(Headers.Authorization, userStorage.Token.ToUpper());
            }
            return client;
        }

        //wylogowanie użytkownika po komunikacie od serwera
        public async Task IfUnauthorized(HttpResponseMessage result)
        {
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                await Unauthorized();
            }
        }

        private async Task Unauthorized()
        {
            await GetLocalStorage().RemoveItemAsync(Headers.Authorization);
            await GetSessionStorage().RemoveItemAsync(Headers.Authorization);
            if (authenticationStateProvider != null)
            {
                await authenticationStateProvider.GetAuthenticationStateAsync();
            }
        }

        //pobranie danych użytkownika w zależności gdzie zostały zapisane
        public async Task<UserStorage?> GetUserStorage()
        {
            UserStorage? user = null;

            var result = await GetUserSession();
            if (result is null)
            {
                result = await GetUserLocal();
            }
            if (result is not null)
            {
                try
                {
                    user = Memy.Shared.Helper.ConvertByteString.ConvertToObject<UserStorage>(result);
                }
                catch (Exception)
                {
                    await Unauthorized();
                    throw;
                }
            }

            return user;
        }
        //pobranie danych zapisanych w local storage
        private async Task<string?> GetUserLocal()
        {
            try
            {
                var resultRav = await GetLocalStorage().GetItemAsStringAsync(Memy.Shared.Helper.Headers.Authorization);
                if (resultRav != null)
                {
                    return resultRav;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }
        //pobranie danych zapisanych w seasion storage
        private async Task<string?> GetUserSession()
        {
            try
            {
                var resultRav = await GetSessionStorage().GetItemAsStringAsync(Memy.Shared.Helper.Headers.Authorization);
                if (resultRav != null)
                {
                    return resultRav;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task GetAcceptedCookie()
        {
            if (!RodoAvailable.AcceptedCookieNotice)
            {
                var response = await _localStorageService.GetItemAsync<bool>(nameof(RodoAvailable.AcceptedCookieNotice));
                RodoAvailable.AcceptedCookieNotice = response;
            }
        }
        public async Task SetAcceptedCookie()
        {
            await _localStorageService.SetItemAsync<bool>(nameof(RodoAvailable.AcceptedCookieNotice), RodoAvailable.AcceptedCookieNotice);
        }

    }
}