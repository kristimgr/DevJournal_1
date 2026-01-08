using Microsoft.Extensions.Logging;
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