using DevJournal.Components.Pages;
using DevJournal.Services;

namespace DevJournal
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			// Get the service from MauiProgram
			var journalService = MauiProgram.Services.GetRequiredService<IJournalService>();

			// Create MainPage with the service
			MainPage = new NavigationPage(new MainPage(journalService))
			{
				BarBackgroundColor = Color.FromArgb("#34495E"),
				BarTextColor = Colors.White
			};
		}
	}
}