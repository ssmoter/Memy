using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Components.Authorization;

using System.Net.Http.Json;

namespace PagesLibrary.Data
{
    public interface IReaction
    {
        Task<ReactionModel?> SetReaction(int reactionId, int value, MyEnums.TypOfReaction typOfReaction);
    }

    public class Reaction : BaseApi, IReaction
    {
        public Reaction(HttpClient httpClient, ILocalStorageService localStorageService, ISessionStorageService sessionStorageService, AuthenticationStateProvider authenticationStateProvider) : base(httpClient, localStorageService, sessionStorageService, authenticationStateProvider)
        {
        }

        public async Task<ReactionModel?> SetReaction(int reactionId, int value, MyEnums.TypOfReaction typOfReaction)
        {
            try
            {
                var client = await SetAuthorizationHeader();
                var react = new ReactionModel(reactionId, value, typOfReaction);
                var result = await client.PostAsJsonAsync(Routes.Reaction, react);
                await IfUnauthorized(result);
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ReactionModel>(json);
                }
                throw new System.NullReferenceException();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}