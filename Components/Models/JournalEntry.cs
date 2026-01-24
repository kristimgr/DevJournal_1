using SQLite;

namespace DevJournal.Components.Models;

public class JournalEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Content { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Ignore]
    public string Preview =>
        Content.Length <= 50
            ? Content
            : Content.Substring(0, 50) + "...";
}
