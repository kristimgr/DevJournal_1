using SQLite;

namespace DevJournal.Services;

public class JournalEntryService
{
    private readonly AppDatabase _appDatabase;
    private SQLiteAsyncConnection _db => _appDatabase.GetConnection();

    public JournalEntryService(AppDatabase database)
    {
        _appDatabase = database;
    }

    public async Task<int> AddEntryAsync(JournalEntry entry)
    {
        await _appDatabase.InitAsync();
        return await _db.InsertAsync(entry);
    }

    public async Task<JournalEntry?> GetEntryByDateAsync(DateTime date)
    {
        await _appDatabase.InitAsync();
        var start = date.Date;
        var end = start.AddDays(1);

        return await _db.Table<JournalEntry>()
                        .Where(e => e.Date >= start && e.Date < end)
                        .FirstOrDefaultAsync();
    }

    public async Task<List<JournalEntry>> GetAllEntriesAsync()
    {
        await _appDatabase.InitAsync();
        return await _db.Table<JournalEntry>()
                        .OrderByDescending(e => e.Date)
                        .ToListAsync();
    }

    public async Task<JournalEntry?> GetEntryByIdAsync(int id)
    {
        await _appDatabase.InitAsync();
        return await _db.Table<JournalEntry>().Where(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> UpdateEntryAsync(JournalEntry entry)
    {
        await _appDatabase.InitAsync();
        entry.UpdatedAt = DateTime.Now;
        return await _db.UpdateAsync(entry);
    }

    public async Task<int> DeleteEntryAsync(JournalEntry entry)
    {
        await _appDatabase.InitAsync();
        return await _db.DeleteAsync(entry);
    }
}
