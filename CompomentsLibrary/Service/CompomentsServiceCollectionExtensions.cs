using CompomentsLibrary.Pages;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompomentsLibrary.Service
{
    public static class ComponentsServiceCollectionExtensions
    {
        //lista serwisów
        public static IServiceCollection AddCompomentsService(this IServiceCollection services)
        {
            services.AddSingleton<PopupListService>();
            services.AddSingleton<ModalPopUpService>();
            services.AddSingleton<ModalAdminMessageService>();

            return services;
        }
    }
}
