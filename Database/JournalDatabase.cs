using DevJournal.Components.Models;

using SQLite;

namespace DevJournal.Database;

public class JournalDatabase
{
    private readonly SQLiteAsyncConnection _db;

    public JournalDatabase(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<User>().Wait();
    }

    public async Task<User?> GetUserAsync()
    {
        return await _db.Table<User>().FirstOrDefaultAsync();
    }

    public async Task InsertUserAsync(User user)
    {
        await _db.InsertAsync(user);
    }
}
