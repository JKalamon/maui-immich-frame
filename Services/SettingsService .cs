using System.Text.Json;

namespace SimpleImmichFrame.Services;

public interface ISettingsService
{
	void LoadSettingsAsync();
	void SaveSettingsAsync();
	AppConfiguration Settings { get; }
}

public class SettingsService : ISettingsService
{
	private const string SETTINGS_KEY = "app_settings";

	public AppConfiguration Settings { get; private set; }

	public SettingsService()
	{
		Settings = new AppConfiguration();
	}

	public void LoadSettingsAsync()
	{
		var settingsJson = Preferences.Get(SETTINGS_KEY, string.Empty);
		if (!string.IsNullOrEmpty(settingsJson))
		{
			try
			{
				Settings = JsonSerializer.Deserialize<AppConfiguration>(settingsJson) ?? new AppConfiguration();
			}
			catch
			{
				Settings = new AppConfiguration();
			}
		}
	}

	public void SaveSettingsAsync()
	{
		var settingsJson = JsonSerializer.Serialize(Settings);
		Preferences.Set(SETTINGS_KEY, settingsJson);
	}
}