using Blazored.LocalStorage;
using Blazored.SessionStorage;

using Memy.Shared.Helper;
using Memy.Shared.Model;

using Newtonsoft.Json;

using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace PagesLibrary.Data.File
{
    public interface IFileManager
    {
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


    }
}
