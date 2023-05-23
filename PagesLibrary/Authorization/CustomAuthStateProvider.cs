using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Model;

using Microsoft.AspNetCore.Components.Authorization;

using System.Security.Claims;

namespace PagesLibrary.Authorization
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly ISessionStorageService _sessionStorageService;
        public CustomAuthStateProvider(ILocalStorageService localStorageService, ISessionStorageService sessionStorageService)
        {
            _localStorageService = localStorageService;
            _sessionStorageService = sessionStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //nowe wystąpienie AuthenticationState które sprawdza czy użytkownik jest zalogowany
            //pusty model == nie zalogowany
            var state = new AuthenticationState(new ClaimsPrincipal());

            UserStorage? user;

            user = await GetSessionStorage();
            if (user == null)
            {
                user = await GetLocalStorage();
            }
            if (user != null)
            {
                //jeżeli jest model zostaje utworzene nowy model z danymi użytkowniak
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.SerialNumber,user.Token)                 
                }, "Memy");

                if (!string.IsNullOrWhiteSpace(user.Role))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
                }
                state = new AuthenticationState(new ClaimsPrincipal(identity));
            }

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }

        private async Task<UserStorage?> GetLocalStorage()
        {
            var resultRav = await _localStorageService.GetItemAsStringAsync(Memy.Shared.Helper.Headers.Authorization);
            if (resultRav != null)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<UserStorage>(resultRav);
                return result;
            }
            return null;
        }
        private async Task<UserStorage?> GetSessionStorage()
        {
            var resultRav = await _sessionStorageService.GetItemAsStringAsync(Memy.Shared.Helper.Headers.Authorization);
            if (resultRav != null)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<UserStorage>(resultRav);
                return result;
            }
            return null;
        }
    }
}
