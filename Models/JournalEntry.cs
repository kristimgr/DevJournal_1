using SQLite;

namespace DevJournal;

public class JournalEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string PrimaryMood { get; set; } = string.Empty;

    public string? Tags { get; set; }
}
