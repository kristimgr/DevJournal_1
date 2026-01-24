using DevJournal.Components.Models;

using DevJournal.Database;
using System.Security.Cryptography;
using System.Text;

namespace DevJournal.Services;

public class AuthService
{
    private readonly JournalDatabase _db;

    public bool IsUnlocked { get; private set; }

    public AuthService(JournalDatabase db)
    {
        _db = db;
    }

    public async Task<bool> HasAccountAsync()
        => await _db.GetUserAsync() != null;

    public async Task<(bool ok, string msg)> RegisterAsync(
        string name,
        string password,
        string confirm)
    {
        name = (name ?? "").Trim();

        if (string.IsNullOrWhiteSpace(name))
            return (false, "Name is required");

        if (string.IsNullOrWhiteSpace(password))
            return (false, "Password is required");

        if (password.Length < 8)
            return (false, "Minimum 8 characters");

        if (password != confirm)
            return (false, "Passwords do not match");

        if (await _db.GetUserAsync() != null)
            return (false, "Account already exists");

        await _db.InsertUserAsync(new User
        {
            Name = name,
            PasswordHash = Hash(password)
        });

        return (true, "Account created");
    }

    public async Task<(bool ok, string msg)> UnlockAsync(string name, string password)
    {
        var user = await _db.GetUserAsync();

        if (user == null)
            return (false, "No account found");

        if (user.Name != name)
            return (false, "Invalid name");

        if (user.PasswordHash != Hash(password))
            return (false, "Invalid password");

        IsUnlocked = true;
        return (true, "Unlocked");
    }

    public void Lock() => IsUnlocked = false;

    private static string Hash(string input)
    {
        using var sha = SHA256.Create();
        return Convert.ToHexString(
            sha.ComputeHash(Encoding.UTF8.GetBytes(input))
        );
    }
}
