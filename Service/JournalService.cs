using DevJournal.Models;

namespace DevJournal.Services
{
    public class JournalService : IJournalService
    {
        private readonly List<JournalEntry> _entries = new();
        private int _nextId = 1;

        public int GetTotalEntries()
        {
            return _entries.Count;
        }

        public bool HasEntryForDate(DateTime date)
        {
            return _entries.Any(e => e.EntryDate.Date == date.Date);
        }

        public JournalEntry? GetEntryByDate(DateTime date)
        {
            return _entries.FirstOrDefault(e => e.EntryDate.Date == date.Date);
        }

        public List<JournalEntry> GetEntriesByMonth(int year, int month)
        {
            return _entries
                .Where(e => e.EntryDate.Year == year && e.EntryDate.Month == month)
                .OrderByDescending(e => e.EntryDate)
                .ToList();
        }

        public List<JournalEntry> GetAllEntries()
        {
            return _entries.OrderByDescending(e => e.EntryDate).ToList();
        }

        public void AddEntry(JournalEntry entry)
        {
            entry.Id = _nextId++;
            entry.CreatedDate = DateTime.Now;
            entry.LastModified = DateTime.Now;
            _entries.Add(entry);
        }

        public void UpdateEntry(JournalEntry entry)
        {
            var existingEntry = _entries.FirstOrDefault(e => e.Id == entry.Id);
            if (existingEntry != null)
            {
                existingEntry.EntryDate = entry.EntryDate;
                existingEntry.Title = entry.Title;
                existingEntry.Content = entry.Content;
                existingEntry.PrimaryMood = entry.PrimaryMood;
                existingEntry.SecondaryMood1 = entry.SecondaryMood1;
                existingEntry.SecondaryMood2 = entry.SecondaryMood2;
                existingEntry.LastModified = DateTime.Now;
            }
        }

        public void DeleteEntry(int id)
        {
            var entry = _entries.FirstOrDefault(e => e.Id == id);
            if (entry != null)
            {
                _entries.Remove(entry);
            }
        }
    }
}