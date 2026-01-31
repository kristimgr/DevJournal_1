using Microsoft.Extensions.Logging;
using DevJournal.Services;

namespace DevJournal;

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

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif

        builder.Services.AddSingleton<AppDatabase>();

        builder.Services.AddSingleton<JournalEntryService>();
        builder.Services.AddSingleton<AppState>();

        return builder.Build();
    }
}
