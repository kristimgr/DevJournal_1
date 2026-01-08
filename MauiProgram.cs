using Microsoft.Extensions.Logging;
<<<<<<< HEAD
using DevJournal.Services;

namespace DevJournal
{
	public static class MauiProgram
	{
		public static IServiceProvider Services { get; private set; } = default!;

		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

#if DEBUG
			builder.Logging.AddDebug();
#endif

			// Register services
			builder.Services.AddSingleton<IJournalService, JournalService>();

			var app = builder.Build();
			Services = app.Services;  // Store the service provider

			return app;
		}
	}
}
=======

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
#endif

		return builder.Build();
	}
}
>>>>>>> fde1b6e0ea6b5f8641aab2f43c7d019b8b89d3d9
