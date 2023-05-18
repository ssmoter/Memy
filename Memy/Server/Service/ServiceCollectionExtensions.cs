using Memy.Server.Data.File;
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
            services.AddTransient<IUserData, LoginData>();
            services.AddSingleton<ITokenManager, TokenManager>();

            services.AddTransient<IAddNewFileModel, AddNewFileModel>();

            return services;
        }
    }
}
