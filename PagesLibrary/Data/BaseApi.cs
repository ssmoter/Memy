using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;
using Memy.Shared.Model;

using System.Net;
using System.Text;

namespace PagesLibrary.Data
{
    public class BaseApi
    {
        private static HttpClient _HttpClient;
        protected string UrlStringName { get; set; } = "https://localhost:7241/api/";
        private readonly ILocalStorageService _localStorageService;
        private readonly ISessionStorageService _sessionStorageService;
        public BaseApi(ILocalStorageService localStorageService,
                       ISessionStorageService sessionStorageService)
        {
            _HttpClient = new HttpClient();
            _HttpClient.BaseAddress = new Uri(UrlStringName);
            _localStorageService = localStorageService;
            _sessionStorageService = sessionStorageService;
        }
        public HttpClient GetHttpClient()
        {
            return _HttpClient;
        }
        public ILocalStorageService GetLocalStorage()
        {
            return _localStorageService;
        }
        public ISessionStorageService GetSessionStorage()
        {
            return _sessionStorageService;
        }
        public async Task<HttpClient> SetAuthorizationHeader()
        {
            UserStorage? userStorage = await GetUserStorage();
            var client = GetHttpClient();
            client.DefaultRequestHeaders.Clear();
            if (userStorage != null)
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
                await GetLocalStorage().RemoveItemAsync(Headers.Authorization);
                await GetSessionStorage().RemoveItemAsync(Headers.Authorization);
            }
        }
        //pobranie danych użytkownika w zależności gdzie zostały zapisane
        public async Task<UserStorage?> GetUserStorage()
        {
            UserStorage? user = null;

            var result = await GetUserSession();
            if (result == null)
            {
                result = await GetUserLocal();
            }
            if (result != null)
            {
                user = Memy.Shared.Helper.ConvertByteString.ConvertToObject<UserStorage>(result);
            }

            return user;
        }
        //pobranie danych zapisanych w local storage
        private async Task<string?> GetUserLocal()
        {
            var resultRav = await _localStorageService.GetItemAsStringAsync(Memy.Shared.Helper.Headers.Authorization);
            if (resultRav != null)
            {
                return resultRav;
            }
            return null;
        }
        //pobranie danych zapisanych w seasion storage
        private async Task<string?> GetUserSession()
        {
            var resultRav = await _sessionStorageService.GetItemAsStringAsync(Memy.Shared.Helper.Headers.Authorization);
            if (resultRav != null)
            {
                return resultRav;
            }
            return null;
        }


    }
}