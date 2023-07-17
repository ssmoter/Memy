using Memy.Server.Data.Admin;
using Memy.Server.Data.Comment;
using Memy.Server.Data.File;
using Memy.Server.Data.Reaction;
using Memy.Server.Data.Reported;
using Memy.Server.Data.SqlDataAccess;
using Memy.Server.Data.User;
using Memy.Server.TokenAuthentication;

namespace Memy.Server.Service
{
    public static class ServiceCollectionExtensions
    {
        //Dodawanie serwisów w innej klasie
        //program.cs jest czytelniejszy
        public static IServiceCollection AddMyService(this IServiceCollection services)
        {
            services.AddTransient<ISqlDataAccess, SqlDataAccess>();
            services.AddTransient<ILoginData, LoginData>();

            services.AddTransient<ITokenManager, TokenManager>();
            services.AddTransient<IAdminTokenManager, AdminTokenManager>();

            services.AddTransient<IAddNewFileModel, AddNewFileModel>();
            services.AddTransient<IReactionDataBase, ReactionDataBase>();
            services.AddTransient<ICommentData, CommentData>();
            services.AddTransient<IUserData, UserData>();
            services.AddTransient<IReportedDataBase, ReportedDataBase>();
            services.AddTransient<IReportedMessagesData, ReportedMessagesData>();
            services.AddTransient<IAdminData, AdminData>();


            return services;
        }
    }
}
