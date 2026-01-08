<<<<<<< HEAD
﻿using DevJournal.Components.Pages;
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
=======
﻿namespace DevJournal;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new MainPage()) { Title = "DevJournal" };
	}
}
>>>>>>> fde1b6e0ea6b5f8641aab2f43c7d019b8b89d3d9
