using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Components.Authorization;

using System.Net.Http.Json;
using System.Text;

namespace PagesLibrary.Data.File
{
    public interface IFileManager
    {
        string GetImg(string name);
        string GetVideo(string name);
        Task<HttpResponseMessage> GetTagAsync();
        Task<HttpResponseMessage> GetTaskModelAsync(int id);
        Task<HttpResponseMessage> GetTaskModelsAsync(int? start, string? categories, int? max, bool? banned, string? dateEnd, string? dateStart, int? orderTyp);
        Task<HttpResponseMessage> PostFileAsync(FileUploadModel file);
        Task<HttpResponseMessage> GetUserTaskModelsAsync(string? name, int? start, int? max, int? orderTyp, bool? banned);
    }

    public class FileManager : BaseApi, IFileManager
    {
        public FileManager(HttpClient httpClient, ILocalStorageService localStorageService, ISessionStorageService sessionStorageService, AuthenticationStateProvider authenticationStateProvider) : base(httpClient, localStorageService, sessionStorageService, authenticationStateProvider)
        {
        }


        //wysyłanie modelu
        public async Task<HttpResponseMessage> PostFileAsync(FileUploadModel file)
        {
            try
            {
                var client = await SetAuthorizationHeader();

                var result = await client.PostAsJsonAsync(Routes.File, file);
                await IfUnauthorized(result);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //pobieranie listy modelów do wyświetlenia
        public async Task<HttpResponseMessage> GetTaskModelsAsync(int? start, string? categories, int? max, bool? banned, string? dateEnd, string? dateStart, int? orderTyp)
        {
            try
            {
                var client = await SetAuthorizationHeader();
                StringBuilder sb = new StringBuilder(Routes.File);

                if (categories != null)
                {
                    sb.Append("/");
                    sb.Append(categories);
                }
                if (start != null)
                {
                    sb.Append("/");
                    sb.Append(start);
                }
                if (max != null || banned != null || dateEnd != null || dateStart != null || orderTyp != null)
                {
                    sb.Append("/?");
                }
                if (max != null)
                {
                    sb.Append("max=");
                    sb.Append(max);
                    sb.Append("&");
                }
                if (banned != null)
                {
                    sb.Append("banned=");
                    sb.Append(banned);
                    sb.Append("&");
                }
                if (dateEnd != null)
                {
                    sb.Append("dateEnd=");
                    sb.Append(dateEnd);
                    sb.Append("&");
                }
                if (dateStart != null)
                {
                    sb.Append("dateStart=");
                    sb.Append(dateStart);
                    sb.Append("&");
                }
                if (orderTyp != null)
                {
                    sb.Append("orderTyp=");
                    sb.Append(orderTyp);
                    sb.Append("&");
                }


                var result = await client.GetAsync(sb.ToString());
                await IfUnauthorized(result);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //pobieranie jednego pliku
        public async Task<HttpResponseMessage> GetTaskModelAsync(int id)
        {
            try
            {
                var client = await SetAuthorizationHeader();
                StringBuilder sb = new StringBuilder(Routes.File);

                sb.Append("/obj/");
                sb.Append(id);

                var result = await client.GetAsync(sb.ToString());
                await IfUnauthorized(result);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //ustawianie adresu z img
        public string GetImg(string name)
        {
            var client = GetHttpClient();
            string url = "";
            if (client is not null)
            {
                if (client.BaseAddress is not null)
                {
                    url = $"{client.BaseAddress.AbsolutePath}{Routes.File}/{Routes.Img}/{name}";
                }
            }
            return url;
        }
        public string GetVideo(string name)
        {
            var client = GetHttpClient();
            string url = "";
            if (client is not null)
            {
                if (client.BaseAddress is not null)
                {
                    url = $"{client.BaseAddress.AbsolutePath}{Routes.File}/{Routes.Video}/{name}";
                }
            }
            return url;
        }
        //pobieranie tagów
        public async Task<HttpResponseMessage> GetTagAsync()
        {
            try
            {
                var client = await SetAuthorizationHeader();
                var result = await client.GetAsync(Routes.Tag);
                await IfUnauthorized(result);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> GetUserTaskModelsAsync(string? name, int? start, int? max, int? orderTyp, bool? banned)
        {
            try
            {
                var client = await SetAuthorizationHeader();
                StringBuilder sb = new StringBuilder(Routes.File);

                sb.Append("/");
                sb.Append(Routes.User);

                if (name != null || start != null || max != null || orderTyp != null || banned != null)
                {
                    sb.Append("?");
                }
                if (name != null)
                {
                    sb.Append("name=");
                    sb.Append(name);
                    sb.Append("&");
                }
                if (start != null)
                {
                    sb.Append("start=");
                    sb.Append(start);
                    sb.Append("&");
                }
                if (max != null)
                {
                    sb.Append("max=");
                    sb.Append(max);
                    sb.Append("&");
                }
                if (orderTyp != null)
                {
                    sb.Append("orderTyp=");
                    sb.Append(orderTyp);
                    sb.Append("&");
                }
                if (banned != null)
                {
                    sb.Append("banned=");
                    sb.Append(banned);
                    sb.Append("&");
                }
                var result = await client.GetAsync(sb.ToString());
                await IfUnauthorized(result);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
