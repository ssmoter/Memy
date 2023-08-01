using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Components.Authorization;

using System.Net.Http.Json;

namespace PagesLibrary.Data
{
    public interface IReported
    {
        Task<ReportedModel> SetReported(ReportedModel reported);
    }

    public class Reported : BaseApi, IReported
    {
        public Reported(HttpClient httpClient, ILocalStorageService localStorageService, ISessionStorageService sessionStorageService, AuthenticationStateProvider authenticationStateProvider) : base(httpClient, localStorageService, sessionStorageService, authenticationStateProvider)
        {
        }

        public async Task<ReportedModel> SetReported(ReportedModel reported)
        {
            try
            {
                var client = await SetAuthorizationHeader();
                var result = await client.PostAsJsonAsync(Routes.Reported, reported);
                await IfUnauthorized(result);
                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var reportedModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportedModel>(json);
                    ArgumentNullException.ThrowIfNull(reportedModel);
                    return reportedModel;
                }
                throw new System.NullReferenceException(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
