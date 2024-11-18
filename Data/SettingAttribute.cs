namespace SimpleImmichFrame.Data;

public class SettingAttribute : Attribute
{
	public string DisplayName { get; set; }

	public string Description { get; set; }

	public string Category { get; set; }

	public double Minimum { get; set; }

	public double Maximum { get; set; }

	public SettingControlType ControlType { get; set; }

	public SettingAttribute(
			string displayName,
			string description = "",
			string category = "General",
			SettingControlType controlType = SettingControlType.Auto)
	{
		DisplayName = displayName;
		Description = description;
		Category = category;
		ControlType = controlType;
		Minimum = 0;
		Maximum = 100;
	}
}

public enum SettingControlType
{
	Auto,
	Text,
	Toggle,
	Slider,
	NumericEntry,
	Picker
}