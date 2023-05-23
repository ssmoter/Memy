using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;
using Memy.Shared.Model;

using System.Net.Http.Json;
using System.Text;

namespace PagesLibrary.Data.File
{
    public interface IFileManager
    {
        string GetImgAsync(string name, string typ);
        Task<HttpResponseMessage> GetTaskModelsAsync(int? start, string? categories, int? max, bool? banned, string? dateEnd, string? dateStart);
        Task<HttpResponseMessage> PostFileAsync(FileUploadModel file);
    }

    public class FileManager : BaseApi, IFileManager
    {
        public FileManager(ILocalStorageService localStorageService, ISessionStorageService sessionStorageService) : base(localStorageService, sessionStorageService)
        {
        }

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
        public async Task<HttpResponseMessage> GetTaskModelsAsync(int? start, string? categories, int? max, bool? banned, string? dateEnd , string? dateStart)
        {
            try
            {
                var client = GetHttpClient();
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
                if (max!=null || banned!=null || dateEnd !=null || dateStart !=null)
                {
                    sb.Append("/?");
                }
                if (max!=null)
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

                var result = await client.GetAsync(sb.ToString());

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //ustawianie adresu z img
        public string GetImgAsync(string name, string typ)
        {
            string url = $"{UrlStringName}{Routes.File}/{Routes.Img}/{name}.{typ}";
            return url;
        }
    }
}
