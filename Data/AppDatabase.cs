using SQLite;

namespace DevJournal;

public class AppDatabase
{
    private readonly SQLiteAsyncConnection _database;
    private bool _initialized = false;

    public AppDatabase()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "journal.db3");
        _database = new SQLiteAsyncConnection(dbPath);
    }

    public async Task InitAsync()
    {
        if (!_initialized)
        {
            await _database.CreateTableAsync<JournalEntry>();
            await _database.CreateTableAsync<DevJournal.Models.Mood>();
            await _database.CreateTableAsync<DevJournal.Models.Tag>();
            _initialized = true;
        }
    }

    public SQLiteAsyncConnection GetConnection() => _database;
}
