using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Components.Authorization;

using System.Net.Http.Json;

namespace PagesLibrary.Data.User
{
    public interface IReportedMessagesApi
    {
        Task<List<ReportedMessagesModel>> GetReportedMessages();
        Task<List<ReportedMessagesModel>> PutReportedMessages(ReportedMessagesModel model);
    }

    public class ReportedMessagesApi : BaseApi, IReportedMessagesApi
    {
        public ReportedMessagesApi(ILocalStorageService localStorageService, ISessionStorageService sessionStorageService, AuthenticationStateProvider authenticationStateProvider = null) : base(localStorageService, sessionStorageService, authenticationStateProvider)
        {
        }

        public async Task<List<ReportedMessagesModel>> GetReportedMessages()
        {
            try
            {
                var client = await SetAuthorizationHeader();

                var result = await client.GetAsync(Routes.ReportedMessages);
                await IfUnauthorized(result);
                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReportedMessagesModel>>(json);
                }
                throw new Exception(json);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ReportedMessagesModel>> PutReportedMessages(ReportedMessagesModel model)
        {
            try
            {
                var client = await SetAuthorizationHeader();

                var result = await client.PutAsJsonAsync(Routes.ReportedMessages, model);
                await IfUnauthorized(result);
                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReportedMessagesModel>>(json);
                }
                throw new Exception(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
