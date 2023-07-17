using Blazored.LocalStorage;
using Blazored.SessionStorage;

using CompomentsLibrary.Service;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

using PagesLibrary.Authorization;
using PagesLibrary.Data;
using PagesLibrary.Data.Admin;
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
            services.AddScoped<BaseApi>();

            services.AddScoped<ILogInOut, LogInOut>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IReaction, Reaction>();
            services.AddScoped<ICommentApi, CommentApi>();
            services.AddScoped<IProfileData, ProfileData>();
            services.AddScoped<IReported, Reported>();
            services.AddScoped<IReportedMessagesApi, ReportedMessagesApi>();


            services.AddScoped<AdminFileApi>();
            services.AddScoped<AdminCommentApi>();


            services.AddSingleton<MainFilePopUpService>();
            services.AddSingleton<LoginPopUpService>();
            services.AddSingleton<CategoriesPopUpServie>();


            services.AddCompomentsService();

            return services;
        }
    }
}
