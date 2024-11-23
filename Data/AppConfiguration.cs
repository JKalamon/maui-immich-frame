namespace SimpleImmichFrame.Data;

// Models/AppConfiguration.cs
public record AppConfiguration
{
	[Setting("Server URL", "The URL of your Immich server e.g. http://photos.myserver.com / http://192.168.0.100:2283", "Server", SettingControlType.Text)]
	public string ImmichServerUrl { get; set; } = "http://photos.myserver.com";

	[Setting("Api Key", """Read more about how to obtain an <a href="https://immich.app/docs/features/command-line-interface#obtain-the-api-key">immich API key</a>.""", "Server", SettingControlType.Text)]
	public string ApiKey { get; set; } = "HerePutYourSecretAPIKey123456789XYZ";

	[Setting("Show Clock", "Toggle clock visibility on/off", "Date & Time", SettingControlType.Toggle)]
	public bool ShowClock { get; set; } = true;

	[Setting("Clock Hour Format", "Set the format for displaying time (e.g. HH:mm)", "Date & Time", SettingControlType.Text)]
	public string ClockHourFormat { get; set; } = "HH:mm";

	[Setting("Clock Font Size", "Adjust the font size of the clock display", "Date & Time", SettingControlType.Slider, Minimum = 12, Maximum = 320)]
	public double ClockFontSize { get; set; } = 72;

	[Setting("Show Date", "Toggle date visibility above the clock", "Date & Time", SettingControlType.Toggle)]
	public bool ShowDate { get; set; } = true;

	[Setting("Date Format", "Set the format for displaying date (e.g. dd.MM.yyyy)", "Date & Time", SettingControlType.Text)]
	public string DateFormat { get; set; } = "dd.MM.yyyy";

	[Setting("Date Font Size", "Adjust the font size of the date display", "Date & Time", SettingControlType.Slider, Minimum = 12, Maximum = 320)]
	public double DateFontSize { get; set; } = 32;

	[Setting("Culture info", """Culture used for date formatting <a href="https://learn.microsoft.com/en-us/bingmaps/rest-services/common-parameters-and-types/supported-culture-codes">whole list</a>.""", "Date & Time", SettingControlType.Text)]
	public string Culture { get; set; } = "pl";

	[Setting("Image duration", "Image display duration in seconds", "Photos", SettingControlType.NumericEntry, Minimum = 1)]
	public double ImageDuration { get; set; } = 15;

	[Setting("Image transition duration", "Image animation transition duration in seconds", "Photos", SettingControlType.NumericEntry, Minimum = 1)]
	public double ImageTransitionDuration { get; set; } = 2;


	[Setting("EnableSleep", "At scheduled time turns screen to black in order to save energy", "Sleep time", SettingControlType.Toggle)]
	public bool EnableSleepTime { get; set; } = true;

	[Setting("Sleep start hour", "At what time screen should go black", "Sleep time", SettingControlType.Slider, Minimum = 0, Maximum = 23)]
	public int SleepStartTime { get; set; } = 1;

	[Setting("Sleep end hour", "At what time screen should go back to display photos", "Sleep time", SettingControlType.Slider, Minimum = 0, Maximum = 23)]
	public int SleepEndTime { get; set; } = 6;
}
