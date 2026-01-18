using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using JournalApp.Components.Service;
using JournalApp.Components.ViewModels;
namespace JournalApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            //registering mudblazor service
            builder.Services.AddMauiBlazorWebView();
            //registering database service
            builder.Services.AddSingleton<DatabaseService>();
            //regitering user serivce 
            builder.Services.AddSingleton<IUserService, UserService>();

            builder.Services.AddSingleton<LoginViewModel>(); 
            builder.Services.AddSingleton<RegisterViewModel>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            //Add MudBlazor services
            builder.Services.AddMudServices();

            
            //builder.Services.AddSingleton<JournalService>();

            return builder.Build();
        }
    }
}