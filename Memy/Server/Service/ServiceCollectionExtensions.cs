using Memy.Server.Data.Admin;
using Memy.Server.Data.Comment;
using Memy.Server.Data.Error;
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
            services.AddScoped<ISqlDataAccess, SqlDataAccess>();
            services.AddScoped<ILoginData, LoginData>();

            services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<IAdminTokenManager, AdminTokenManager>();
            services.AddSingleton<TokenList>();

            services.AddScoped<IAddNewFileModel, AddNewFileModel>();
            services.AddScoped<IReactionDataBase, ReactionDataBase>();
            services.AddScoped<ICommentData, CommentData>();
            services.AddScoped<IUserData, UserData>();
            services.AddScoped<IReportedDataBase, ReportedDataBase>();
            services.AddScoped<IReportedMessagesData, ReportedMessagesData>();
            services.AddScoped<IAdminData, AdminData>();

            services.AddScoped<ILogData, LogData>();

            return services;
        }
    }
}
