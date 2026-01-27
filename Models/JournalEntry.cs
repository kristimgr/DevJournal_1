using SQLite;

namespace DevJournal;

public class JournalEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string PrimaryMood { get; set; } = string.Empty;

    public string SecondaryMoods { get; set; } = string.Empty; // Comma-separated

    public string Category { get; set; } = string.Empty;

    public string? Tags { get; set; }
}
