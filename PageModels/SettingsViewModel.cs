using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleImmichFrame.Models;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;

namespace SimpleImmichFrame.PageModels;

public partial class SettingsViewModel : ObservableObject
{
	private readonly ISettingsService settingsService;
	private Dictionary<string, object> ssettingValues = new();

	[ObservableProperty]
	private List<SettingsGroup> settingsGroups = new();

	public ICommand SaveSettingsCommand { get; }

	public SettingsViewModel(ISettingsService settingsService)
	{
		this.settingsService = settingsService;
		SaveSettingsCommand = new RelayCommand(SaveSettingsAsync);
	}

	[RelayCommand]
	private Task Appearing()
	{
		LoadSettings();
		return Task.CompletedTask;
	}

	private void LoadSettings()
	{
		List<SettingsGroup> list = [];

		var config = settingsService.Settings;
		var properties = typeof(AppConfiguration).GetProperties()
				.Where(p => p.GetCustomAttribute<SettingAttribute>() != null)
				.GroupBy(p => p.GetCustomAttribute<SettingAttribute>().Category);

		foreach (var group in properties)
		{
			var settingsGroup = new SettingsGroup { Name = group.Key };

			foreach (var prop in group)
			{
				var attribute = prop.GetCustomAttribute<SettingAttribute>();
				var value = prop.GetValue(config);
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
						prop.SetValue(config, Convert.ChangeType(newValue, prop.PropertyType));
						SaveSettingsAsync();
					}
				});
			}

			list.Add(settingsGroup);
		}

		this.SettingsGroups = list;
	}

	private void SaveSettingsAsync()
	{
		settingsService.SaveSettingsAsync();
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
