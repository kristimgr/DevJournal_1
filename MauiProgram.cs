using DevJournal.Database;
using DevJournal.Services;
using Microsoft.Extensions.Logging;

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

        // ✅ MAUI Blazor
        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif

        // ✅ SQLite database
        var dbPath = Path.Combine(
            FileSystem.AppDataDirectory,
            "journal.db"
        );

        builder.Services.AddSingleton(
            new JournalDatabase(dbPath)
        );

        // ✅ Auth service
        builder.Services.AddSingleton<AuthService>();

        return builder.Build();
    }
}
