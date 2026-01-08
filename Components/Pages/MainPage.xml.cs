using DevJournal.Services;
using DevJournal.Pages;
using DevJournal.Models;

namespace DevJournal.Components.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly IJournalService journalService;

        public MainPage(IJournalService service)
        {
            InitializeComponent();
            journalService = service;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateStatistics();
            LoadRecentEntries();
        }

        private void UpdateStatistics()
        {
            var totalEntries = journalService.GetTotalEntries();
            totalEntriesLabel.Text = $"Total Entries: {totalEntries}";

            var hasEntryToday = journalService.HasEntryForDate(DateTime.Today);
            if (hasEntryToday)
            {
                todayStatusLabel.Text = "✅ You've written today!";
                todayStatusLabel.TextColor = Color.FromArgb("#2ECC71");
            }
            else
            {
                todayStatusLabel.Text = "⏳ No entry for today yet";
                todayStatusLabel.TextColor = Color.FromArgb("#E74C3C");
            }
        }

        private void LoadRecentEntries()
        {
            var allEntries = journalService.GetAllEntries();
            var recentEntries = allEntries.Take(5).ToList(); // Show last 5 entries
            recentEntriesCollection.ItemsSource = recentEntries;
        }

        private async void OnEntrySelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is JournalEntry entry)
            {
                // Deselect
                recentEntriesCollection.SelectedItem = null;

                // Navigate to edit the entry
                await Navigation.PushAsync(new EntryEditorPage(journalService, entry.EntryDate));
            }
        }

        private async void OnTodayEntryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntryEditorPage(journalService, DateTime.Today));
        }

        private async void OnCalendarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CalendarPage(journalService));
        }

        private async void OnAboutClicked(object sender, EventArgs e)
        {
            await DisplayAlert("About Personal Journal",
                "Personal Journal Application\n" +
                "Version 1.0 - Milestone 1\n\n" +
                "Developed for CS6004NP\n" +
                "Module Leader: Mr. Sudip Bhandari\n\n" +
                "Features:\n" +
                "✅ Journal Entry Management (CRUD)\n" +
                "✅ Mood Tracking\n" +
                "✅ Rich Text Editor\n" +
                "✅ Calendar Navigation",
                "OK");
        }
    }
}