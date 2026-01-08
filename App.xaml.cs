using DevJournal.Components.Pages;
using DevJournal.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DevJournal
{
	public partial class App : Application
	{
		public App(IServiceProvider serviceProvider)
		{
			InitializeComponent();

			var journalService = serviceProvider.GetRequiredService<IJournalService>();

			MainPage = new NavigationPage(new MainPage())
			{
				BarBackgroundColor = Color.FromArgb("#34495E"),
				BarTextColor = Colors.White
			};
		}
	}
}