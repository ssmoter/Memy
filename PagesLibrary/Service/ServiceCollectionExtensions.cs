using Blazored.LocalStorage;
using Blazored.SessionStorage;

using CompomentsLibrary.Service;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

using PagesLibrary.Authorization;
using PagesLibrary.Data;
using PagesLibrary.Data.Comment;
using PagesLibrary.Data.File;
using PagesLibrary.Data.User;

namespace PagesLibrary.Service
{
    public static class ServiceCollectionExtensions
    {
        //Dodawanie serwisów w innej klasie
        //program.cs jest czytelniejszy
        public static IServiceCollection AddMyService(this IServiceCollection services)
        {
            services.AddBlazoredLocalStorageAsSingleton();
            services.AddBlazoredSessionStorageAsSingleton();
            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            services.AddSingleton<BaseApi>();

            services.AddSingleton<ILogInOut, LogInOut>();
            services.AddSingleton<IFileManager, FileManager>();
            services.AddSingleton<IReaction, Reaction>();
            services.AddSingleton<ICommentApi, CommentApi>();

            services.AddSingleton<MainFilePopUpService>();

            services.AddCompomentsService();

            return services;
        }
    }
}
