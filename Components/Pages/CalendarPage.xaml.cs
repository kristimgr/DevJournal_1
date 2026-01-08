using DevJournal.Models;
using DevJournal.Services;

namespace DevJournal.Pages
{
    public partial class CalendarPage : ContentPage
    {
        private readonly IJournalService _journalService;
        private DateTime _currentMonth;
        private List<JournalEntry> _monthEntries = new();
        private JournalEntry? _selectedEntry;

        public CalendarPage(IJournalService journalService)
        {
            InitializeComponent();

            _journalService = journalService;
            _currentMonth = DateTime.Today;

            InitializeDayHeaders();
            LoadMonthEntries();
            CreateCalendarGrid();
        }

        private void InitializeDayHeaders()
        {
            string[] dayNames = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

            for (int i = 0; i < 7; i++)
            {
                var label = new Label
                {
                    Text = dayNames[i],
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.White,
                    BackgroundColor = Color.FromArgb("#34495E"),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };
                Grid.SetColumn(label, i);
                dayHeadersGrid.Children.Add(label);
            }
        }

        private void LoadMonthEntries()
        {
            _monthEntries = _journalService.GetEntriesByMonth(_currentMonth.Year, _currentMonth.Month);
            monthYearLabel.Text = _currentMonth.ToString("MMMM yyyy");
        }

        private void CreateCalendarGrid()
        {
            calendarGrid.Children.Clear();
            calendarGrid.RowDefinitions.Clear();

            var firstDayOfMonth = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            int totalDays = startDayOfWeek + lastDayOfMonth.Day;
            int rows = (int)Math.Ceiling(totalDays / 7.0);

            for (int i = 0; i < rows; i++)
            {
                calendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            }

            calendarGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < 7; i++)
            {
                calendarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            int row = 0;
            int col = 0;

            for (int i = startDayOfWeek - 1; i >= 0; i--)
            {
                DateTime date = firstDayOfMonth.AddDays(-i - 1);
                CreateDayButton(date, row, col, true);
                col++;
            }

            for (int day = 1; day <= lastDayOfMonth.Day; day++)
            {
                DateTime date = new DateTime(_currentMonth.Year, _currentMonth.Month, day);
                CreateDayButton(date, row, col, false);
                col++;

                if (col > 6)
                {
                    col = 0;
                    row++;
                }
            }

            int remainingCells = (rows * 7) - (startDayOfWeek + lastDayOfMonth.Day);
            for (int i = 1; i <= remainingCells; i++)
            {
                DateTime date = lastDayOfMonth.AddDays(i);
                CreateDayButton(date, row, col, true);
                col++;

                if (col > 6)
                {
                    col = 0;
                    row++;
                }
            }
        }

        private void CreateDayButton(DateTime date, int row, int col, bool isOtherMonth)
        {
            var entry = _monthEntries.FirstOrDefault(e => e.EntryDate.Date == date.Date);

            string displayText = date.Day.ToString();
            if (entry != null)
            {
                string emoji = MoodOptions.GetMoodEmoji(entry.PrimaryMood);
                displayText = $"{date.Day}\n{emoji}";
            }

            var button = new Button
            {
                Text = displayText,
                FontSize = 14,
                CornerRadius = 8,
                Padding = new Thickness(5)
            };

            if (isOtherMonth)
            {
                button.BackgroundColor = Color.FromArgb("#ECF0F1");
                button.TextColor = Color.FromArgb("#BDC3C7");
            }
            else if (date.Date == DateTime.Today)
            {
                button.BackgroundColor = Color.FromArgb("#3498DB");
                button.TextColor = Colors.White;
                button.FontAttributes = FontAttributes.Bold;
            }
            else if (entry != null)
            {
                button.BackgroundColor = Color.FromArgb("#E8F8F5");
                button.TextColor = Color.FromArgb("#2C3E50");
                button.BorderColor = Color.FromArgb("#2ECC71");
                button.BorderWidth = 2;
            }
            else
            {
                button.BackgroundColor = Colors.White;
                button.TextColor = Color.FromArgb("#2C3E50");
            }

            button.Clicked += (s, e) => OnDayClicked(date);

            Grid.SetRow(button, row);
            Grid.SetColumn(button, col);
            calendarGrid.Children.Add(button);
        }

        private void OnDayClicked(DateTime date)
        {
            var entry = _monthEntries.FirstOrDefault(e => e.EntryDate.Date == date.Date);

            if (entry != null)
            {
                ShowEntryPreview(entry);
            }
            else
            {
                previewFrame.IsVisible = true;
                previewTitleLabel.Text = "No entry for this date";
                previewDateLabel.Text = date.ToString("dddd, MMMM dd, yyyy");
                previewMoodLabel.Text = "";
                previewContentLabel.Text = "";
                _selectedEntry = null;
            }
        }

        private void ShowEntryPreview(JournalEntry entry)
        {
            _selectedEntry = entry;
            previewFrame.IsVisible = true;

            previewTitleLabel.Text = entry.Title;
            previewDateLabel.Text = entry.EntryDate.ToString("dddd, MMMM dd, yyyy");

            string moodText = $"{MoodOptions.GetMoodEmoji(entry.PrimaryMood)} {entry.PrimaryMood}";
            if (!string.IsNullOrEmpty(entry.SecondaryMood1))
                moodText += $"  {MoodOptions.GetMoodEmoji(entry.SecondaryMood1)} {entry.SecondaryMood1}";
            if (!string.IsNullOrEmpty(entry.SecondaryMood2))
                moodText += $"  {MoodOptions.GetMoodEmoji(entry.SecondaryMood2)} {entry.SecondaryMood2}";

            previewMoodLabel.Text = moodText;

            string preview = entry.Content.Length > 200
                ? entry.Content.Substring(0, 200) + "..."
                : entry.Content;
            previewContentLabel.Text = preview;
        }

        private void OnPreviousMonthClicked(object sender, EventArgs e)
        {
            _currentMonth = _currentMonth.AddMonths(-1);
            LoadMonthEntries();
            CreateCalendarGrid();
            previewFrame.IsVisible = false;
        }

        private void OnNextMonthClicked(object sender, EventArgs e)
        {
            _currentMonth = _currentMonth.AddMonths(1);
            LoadMonthEntries();
            CreateCalendarGrid();
            previewFrame.IsVisible = false;
        }

        private async void OnNewEntryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntryEditorPage(_journalService, DateTime.Today));
        }

        private async void OnEditEntryClicked(object sender, EventArgs e)
        {
            if (_selectedEntry != null)
            {
                await Navigation.PushAsync(new EntryEditorPage(_journalService, _selectedEntry.EntryDate));
            }
        }

        private async void OnDeleteEntryClicked(object sender, EventArgs e)
        {
            if (_selectedEntry == null)
                return;

            bool confirm = await DisplayAlert("Confirm Delete",
                "Are you sure you want to delete this entry?",
                "Yes", "No");

            if (confirm)
            {
                _journalService.DeleteEntry(_selectedEntry.Id);
                await DisplayAlert("Success", "Entry deleted successfully!", "OK");
                LoadMonthEntries();
                CreateCalendarGrid();
                previewFrame.IsVisible = false;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadMonthEntries();
            CreateCalendarGrid();
            previewFrame.IsVisible = false;
        }
    }
}