using System.Text.Json;

namespace SimpleImmichFrame.Services;

public interface ISettingsService
{
	void LoadSettingsAsync();
	void SaveSettingsAsync(AppConfiguration settings);
	AppConfiguration Settings { get; }

	event EventHandler<AppConfiguration>? SettingsChanged;
}

public class SettingsService : ISettingsService
{
	private const string SETTINGS_KEY = "app_settings";

	public event EventHandler<AppConfiguration>? SettingsChanged;

	public AppConfiguration Settings => this.settings with { };

	private AppConfiguration settings;

	public SettingsService()
	{
		settings = new AppConfiguration();
	}

	public void LoadSettingsAsync()
	{
		var settingsJson = Preferences.Get(SETTINGS_KEY, string.Empty);
		if (!string.IsNullOrEmpty(settingsJson))
		{
			try
			{
				settings = JsonSerializer.Deserialize<AppConfiguration>(settingsJson) ?? new AppConfiguration();
			}
			catch
			{
				settings = new AppConfiguration();
			}
		}
	}

	public void SaveSettingsAsync(AppConfiguration settings)
	{
		this.settings = settings;
		var settingsJson = JsonSerializer.Serialize(settings);
		Preferences.Set(SETTINGS_KEY, settingsJson);
		SettingsChanged?.Invoke(this, Settings);
	}


}