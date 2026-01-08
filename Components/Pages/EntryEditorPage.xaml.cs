using DevJournal.Models;
using DevJournal.Services;

namespace DevJournal.Pages
{
    public partial class EntryEditorPage : ContentPage
    {
        private readonly IJournalService _journalService;
        private DateTime _entryDate;
        private JournalEntry? _currentEntry;

        public EntryEditorPage(IJournalService journalService, DateTime selectedDate)
        {
            InitializeComponent();
            _journalService = journalService;
            _entryDate = selectedDate;

            InitializeMoodPickers();
            LoadExistingEntry();
        }

        private void InitializeMoodPickers()
        {
            var moods = MoodOptions.GetAllMoods();

            foreach (var mood in moods)
            {
                primaryMoodPicker.Items.Add(mood);
                secondaryMood1Picker.Items.Add(mood);
                secondaryMood2Picker.Items.Add(mood);
            }
        }

        private void LoadExistingEntry()
        {
            _currentEntry = _journalService.GetEntryByDate(_entryDate);

            if (_currentEntry != null)
            {
                entryDatePicker.Date = _currentEntry.EntryDate;
                titleEntry.Text = _currentEntry.Title;
                contentEditor.Text = _currentEntry.Content;

                if (!string.IsNullOrEmpty(_currentEntry.PrimaryMood))
                    primaryMoodPicker.SelectedItem = _currentEntry.PrimaryMood;

                if (!string.IsNullOrEmpty(_currentEntry.SecondaryMood1))
                    secondaryMood1Picker.SelectedItem = _currentEntry.SecondaryMood1;

                if (!string.IsNullOrEmpty(_currentEntry.SecondaryMood2))
                    secondaryMood2Picker.SelectedItem = _currentEntry.SecondaryMood2;

                deleteButton.IsVisible = true;
                UpdateWordCount();
            }
            else
            {
                entryDatePicker.Date = _entryDate;
                deleteButton.IsVisible = false;
            }
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            _entryDate = e.NewDate;
            LoadExistingEntry();
        }

        private void OnContentChanged(object sender, TextChangedEventArgs e)
        {
            UpdateWordCount();
        }

        private void UpdateWordCount()
        {
            if (string.IsNullOrWhiteSpace(contentEditor.Text))
            {
                wordCountLabel.Text = "Words: 0";
                return;
            }

            int wordCount = contentEditor.Text.Split(new[] { ' ', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries).Length;
            wordCountLabel.Text = $"Words: {wordCount}";
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(titleEntry.Text))
            {
                await DisplayAlert("Error", "Please enter a title.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(contentEditor.Text))
            {
                await DisplayAlert("Error", "Please write some content.", "OK");
                return;
            }

            if (primaryMoodPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please select a primary mood.", "OK");
                return;
            }

            try
            {
                if (_currentEntry != null)
                {
                    _currentEntry.EntryDate = entryDatePicker.Date;
                    _currentEntry.Title = titleEntry.Text;
                    _currentEntry.Content = contentEditor.Text;
                    _currentEntry.PrimaryMood = primaryMoodPicker.SelectedItem.ToString() ?? "";
                    _currentEntry.SecondaryMood1 = secondaryMood1Picker.SelectedItem?.ToString() ?? "";
                    _currentEntry.SecondaryMood2 = secondaryMood2Picker.SelectedItem?.ToString() ?? "";
                    _currentEntry.LastModified = DateTime.Now;

                    _journalService.UpdateEntry(_currentEntry);
                    await DisplayAlert("Success", "Entry updated successfully!", "OK");
                }
                else
                {
                    var newEntry = new JournalEntry
                    {
                        EntryDate = entryDatePicker.Date,
                        Title = titleEntry.Text,
                        Content = contentEditor.Text,
                        PrimaryMood = primaryMoodPicker.SelectedItem.ToString() ?? "",
                        SecondaryMood1 = secondaryMood1Picker.SelectedItem?.ToString() ?? "",
                        SecondaryMood2 = secondaryMood2Picker.SelectedItem?.ToString() ?? "",
                        CreatedDate = DateTime.Now,
                        LastModified = DateTime.Now
                    };

                    _journalService.AddEntry(newEntry);
                    await DisplayAlert("Success", "Entry saved successfully!", "OK");
                }

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save entry: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (_currentEntry == null)
                return;

            bool confirm = await DisplayAlert("Confirm Delete",
                "Are you sure you want to delete this entry?",
                "Yes", "No");

            if (confirm)
            {
                try
                {
                    _journalService.DeleteEntry(_currentEntry.Id);
                    await DisplayAlert("Success", "Entry deleted successfully!", "OK");
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete entry: {ex.Message}", "OK");
                }
            }
        }
    }
}