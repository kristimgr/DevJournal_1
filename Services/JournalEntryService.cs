using SQLite;

namespace DevJournal.Services;

public class JournalEntryService
{
    private readonly SQLiteAsyncConnection _db;

    public JournalEntryService(AppDatabase database)
    {
        _db = database.GetConnection();
    }

    public async Task<int> AddEntryAsync(JournalEntry entry)
    {
        return await _db.InsertAsync(entry);
    }

    public async Task<JournalEntry?> GetEntryByDateAsync(DateTime date)
    {
        var start = date.Date;
        var end = start.AddDays(1);

        return await _db.Table<JournalEntry>()
                        .Where(e => e.Date >= start && e.Date < end)
                        .FirstOrDefaultAsync();
    }

    public async Task<List<JournalEntry>> GetAllEntriesAsync()
    {
        return await _db.Table<JournalEntry>()
                        .OrderByDescending(e => e.Date)
                        .ToListAsync();
    }
}
