using SQLite;

namespace DevJournal.Components.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string PasswordHash { get; set; } = "";
}
