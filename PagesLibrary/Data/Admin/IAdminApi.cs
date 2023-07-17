using Memy.Shared.Helper;
using Memy.Shared.Model;

namespace PagesLibrary.Data.Admin
{
    public interface IAdminApi
    {
        Task<HttpResponseMessage> Ban(int id, ReportedMessagesModel reported);
        Task<HttpResponseMessage> Delete(int id, ReportedMessagesModel reported);
        Task<HttpResponseMessage> UpdateCategory(int id, string category, ReportedMessagesModel reported);
    }
}