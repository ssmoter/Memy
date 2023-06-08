using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;
using Memy.Shared.Model;

using System.Net.Http.Json;

namespace PagesLibrary.Data
{
    public interface IReaction
    {
        Task<ReactionModel?> SetReaction(int reactionId, int value, MyEnums.TypOfReaction typOfReaction);
    }

    public class Reaction : BaseApi, IReaction
    {
        public Reaction(ILocalStorageService localStorageService, ISessionStorageService sessionStorageService) : base(localStorageService, sessionStorageService)
        {
        }
        public async Task<ReactionModel?> SetReaction(int reactionId, int value, MyEnums.TypOfReaction typOfReaction)
        {
            try
            {
                var client = await SetAuthorizationHeader();
                var react = new ReactionModel(reactionId, value, typOfReaction);
                var result = await client.PostAsJsonAsync(Routes.Reaction, react);
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