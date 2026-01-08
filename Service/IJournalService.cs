using DevJournal.Models;

namespace DevJournal.Services
{
    public interface IJournalService
    {
        int GetTotalEntries();
        bool HasEntryForDate(DateTime date);
        JournalEntry? GetEntryByDate(DateTime date);
        List<JournalEntry> GetEntriesByMonth(int year, int month);
        List<JournalEntry> GetAllEntries();

        void AddEntry(JournalEntry entry);
        void UpdateEntry(JournalEntry entry);
        void DeleteEntry(int id);
    }
}