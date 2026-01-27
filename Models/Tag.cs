using SQLite;

namespace DevJournal.Models;

public class Tag
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = ""; // Optional: Work, Hobbies, etc.
}
