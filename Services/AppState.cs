using Microsoft.Maui.Storage;

namespace DevJournal.Services;

public class AppState
{
    private const string ThemeKey = "app_theme";
    private const string PinKey = "user_pin";
    private const string PinEnabledKey = "pin_enabled";

    public event Action? OnChange;

    public string CurrentTheme { get; private set; } = "light";
    public bool IsDark => CurrentTheme == "dark";
    
    public bool IsPinSet => Preferences.Get(PinEnabledKey, false);
    public bool IsLocked { get; private set; } = true;

    public Task InitializeAsync()
    {
        // Load Theme
        CurrentTheme = Preferences.Get(ThemeKey, "light");
        
        // Check Lock Status
        if (!IsPinSet)
        {
            IsLocked = false;
        }
        else
        {
            IsLocked = true;
        }
        
        NotifyStateChanged();
        return Task.CompletedTask;
    }

    public void SetTheme(string theme)
    {
        CurrentTheme = theme;
        Preferences.Set(ThemeKey, theme);
        NotifyStateChanged();
    }

    public async Task SetPinAsync(string pin)
    {
        await SecureStorage.SetAsync(PinKey, pin);
        Preferences.Set(PinEnabledKey, true);
        NotifyStateChanged();
    }

    public async Task<bool> VerifyPinAsync(string pin)
    {
        var storedPin = await SecureStorage.GetAsync(PinKey);
        return storedPin == pin;
    }

    public void Unlock()
    {
        IsLocked = false;
        NotifyStateChanged();
    }
    
    public void Lock()
    {
        if (IsPinSet)
        {
            IsLocked = true;
            NotifyStateChanged();
        }
    }

    public void RemovePin()
    {
        SecureStorage.Remove(PinKey);
        Preferences.Set(PinEnabledKey, false);
        IsLocked = false;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
