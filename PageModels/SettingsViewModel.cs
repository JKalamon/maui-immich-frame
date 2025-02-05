using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Reflection;

namespace SimpleImmichFrame.PageModels;

public partial class SettingsViewModel : ObservableObject
{
	private readonly ISettingsService settingsService;
	private readonly IServiceProvider provider;
	private readonly INavigation navigation;
	private Dictionary<string, object> ssettingValues = new();
	private AppConfiguration settings = new();

	[ObservableProperty]
	private List<SettingsGroup> settingsGroups = new();

	public SettingsViewModel(ISettingsService settingsService, IServiceProvider provider, INavigation navigation)
	{
		this.settingsService = settingsService;
		this.provider = provider;
		this.navigation = navigation;
		LoadSettings();
	}

	[RelayCommand]
	private async Task SaveSettings()
	{
		SaveSettingsAsync();
		await this.navigation.PushAsync(provider.GetRequiredService<PhotoPage>());
		this.navigation.RemovePage(this.navigation.NavigationStack.First());
	}

	[RelayCommand]
	private async Task Cancel()
	{
		await this.navigation.PushAsync(provider.GetRequiredService<PhotoPage>());
		this.navigation.RemovePage(this.navigation.NavigationStack.First());
	}

	private void LoadSettings()
	{
		List<SettingsGroup> list = [];

		this.settings = settingsService.Settings;
		var properties = typeof(AppConfiguration).GetProperties()
				.Where(p => p.GetCustomAttribute<SettingAttribute>() != null)
				.GroupBy(p => p.GetCustomAttribute<SettingAttribute>().Category);

		foreach (var group in properties)
		{
			var settingsGroup = new SettingsGroup { Name = group.Key };

			foreach (var prop in group)
			{
				var attribute = prop.GetCustomAttribute<SettingAttribute>();
				var value = prop.GetValue(this.settings);
				ssettingValues[prop.Name] = value;

				settingsGroup.Settings.Add(new SettingItem
				{
					PropertyName = prop.Name,
					DisplayName = attribute.DisplayName,
					Description = attribute.Description,
					Value = value,
					PropertyType = prop.PropertyType,
					ControlType = attribute.ControlType,
					Minimum = attribute.Minimum,
					Maximum = attribute.Maximum,
					ValueChanged = (newValue) =>
					{
						ssettingValues[prop.Name] = newValue;
						prop.SetValue(this.settings, Convert.ChangeType(newValue, prop.PropertyType));						
					}
				});
			}

			list.Add(settingsGroup);
		}

		this.SettingsGroups = list;
	}

	private void SaveSettingsAsync()
	{
		settingsService.SaveSettingsAsync(this.settings);
	}
}

public class SettingsGroup
{
	public string Name { get; set; }

	public string Bizon { get; set; } = "Bizon";
	public ObservableCollection<SettingItem> Settings { get; } = new();
}

public class SettingItem
{
	public string PropertyName { get; set; }
	public string DisplayName { get; set; }
	public string Description { get; set; }
	public object Value { get; set; }
	public Type PropertyType { get; set; }
	public SettingControlType ControlType { get; set; }
	public double Minimum { get; set; }
	public double Maximum { get; set; }
	public Action<object> ValueChanged { get; set; }
}
