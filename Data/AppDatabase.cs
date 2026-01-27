using SQLite;

namespace DevJournal;

public class AppDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public AppDatabase()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "journal.db3");
        _database = new SQLiteAsyncConnection(dbPath);

        _database.CreateTableAsync<JournalEntry>().Wait();
    }

    public SQLiteAsyncConnection GetConnection() => _database;
}
