using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using JournalApp.Components.Service;
using JournalApp.Components.Models;
namespace JournalApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            SQLitePCL.Batteries_V2.Init();
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
            //registering journal entry service
            builder.Services.AddSingleton<IJournalEntryService, JournalEntryService>();
            //registering analytics service 
            builder.Services.AddSingleton<AnalyticsService>();
            //registering mood service 
            builder.Services.AddSingleton<MoodService>();
            //registering tag service 
            builder.Services.AddSingleton<TagService>();
            //registering category service 
            builder.Services.AddSingleton<CategoryService>();
            //registering streak service 
            builder.Services.AddSingleton<StreakService>();
            
            

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