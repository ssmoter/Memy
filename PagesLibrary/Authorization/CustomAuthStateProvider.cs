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

            try
            {
                user = await GetUserStorage();
                if (user != null)
                {
                    //jeżeli jest model zostaje utworzene nowy model z danymi użytkowniak
                    var identity = new ClaimsIdentity("Memy");

                    if (!string.IsNullOrWhiteSpace(user.UserName))
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                    }
                    if (!string.IsNullOrWhiteSpace(user.Token))
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Authentication, user.Token));
                    }
                    if (!string.IsNullOrWhiteSpace(user.Role))
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
                    }
                    state = new AuthenticationState(new ClaimsPrincipal(identity));
                }
            }
            catch (Exception)
            { }


            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
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
