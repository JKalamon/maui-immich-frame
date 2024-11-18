using System.Text.RegularExpressions;

namespace SimpleImmichFrame.Controls;

public class DynamicSettingControl : ContentView
{
	public static readonly BindableProperty SettingProperty =
				BindableProperty.Create(nameof(Setting), typeof(SettingItem), typeof(DynamicSettingControl),
						propertyChanged: OnSettingChanged);

	public SettingItem Setting
	{
		get => (SettingItem)GetValue(SettingProperty);
		set => SetValue(SettingProperty, value);
	}

	public DynamicSettingControl()
	{
	}

	private static void OnSettingChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (DynamicSettingControl)bindable;
		var setting = (SettingItem)newValue;
		control.CreateControl(setting);
	}

	private void CreateControl(SettingItem setting)
	{
		var controlType = setting.ControlType;
		if (controlType == SettingControlType.Auto)
		{
			controlType = DetermineControlType(setting.PropertyType);
		}

		View control = controlType switch
		{
			SettingControlType.Toggle => CreateToggle(setting),
			SettingControlType.Slider => CreateSlider(setting),
			SettingControlType.Text => CreateEntry(setting),
			SettingControlType.NumericEntry => CreateNumericEntry(setting),
			_ => new Label { Text = "Unsupported type" }
		};

		var layout = new VerticalStackLayout
		{
			Spacing = 5,
			Children =
						{
								new Label
								{
										Text = setting.DisplayName,
										FontAttributes = FontAttributes.Bold
								},
								this.ConvertToFormattedLabel(setting.Description),
								new Label
								{
										Text = setting.Description,
										FontSize = 12,
										TextColor = Colors.Gray
								},
								control
						}
		};

		Content = new Border
		{
			Padding = new Thickness(10),
			Content = layout
		};
	}

	private static readonly Regex LinkRegex = new(@"<a\s+href=""([^""]+)""[^>]*>([^<]+)</a>", RegexOptions.Compiled);

	public Label ConvertToFormattedLabel(string htmlText)
	{
		if (string.IsNullOrEmpty(htmlText) || !htmlText.Contains("<a"))
		{
			return new Label { Text = htmlText };
		}

		var label = new Label();
		var formattedString = new FormattedString();
		int lastIndex = 0;

		foreach (Match match in LinkRegex.Matches(htmlText))
		{
			// Add text before the link
			if (match.Index > lastIndex)
			{
				formattedString.Spans.Add(new Span
				{
					Text = htmlText[lastIndex..match.Index]
				});
			}

			// Create the link span
			var linkSpan = new Span
			{
				Text = match.Groups[2].Value, // Link text
				TextColor = Colors.Blue,
				TextDecorations = TextDecorations.Underline
			};

			var gestureRecognizer = new TapGestureRecognizer
			{
				Command = new Command<string>(async (url) => await Launcher.OpenAsync(url)),
				CommandParameter = match.Groups[1].Value // URL
			};

			linkSpan.GestureRecognizers.Add(gestureRecognizer);
			formattedString.Spans.Add(linkSpan);

			lastIndex = match.Index + match.Length;
		}

		// Add any remaining text after the last link
		if (lastIndex < htmlText.Length)
		{
			formattedString.Spans.Add(new Span
			{
				Text = htmlText[lastIndex..]
			});
		}

		label.FormattedText = formattedString;
		return label;
	}

	private SettingControlType DetermineControlType(Type propertyType)
	{
		return propertyType switch
		{
			Type t when t == typeof(bool) => SettingControlType.Toggle,
			Type t when t == typeof(int) || t == typeof(double) || t == typeof(float)
					=> SettingControlType.Slider,
			Type t when t == typeof(string) => SettingControlType.Text,
			_ => SettingControlType.Text
		};
	}

	private Switch CreateToggle(SettingItem setting)
	{
		var toggle = new Switch
		{
			IsToggled = (bool)setting.Value,
			HorizontalOptions = LayoutOptions.Start
		};
		toggle.Toggled += (s, e) => setting.ValueChanged(e.Value);
		return toggle;
	}

	private Slider CreateSlider(SettingItem setting)
	{
		var slider = new Slider
		{
			Value = Convert.ToDouble(setting.Value),
			Minimum = setting.Minimum,
			Maximum = setting.Maximum,
			MinimumTrackColor = Colors.Blue,
			MaximumTrackColor = Colors.Gray,
			ThumbColor = Colors.Blue,
			WidthRequest = 250
		};
		slider.ValueChanged += (s, e) => setting.ValueChanged(e.NewValue);
		return slider;
	}

	private Entry CreateEntry(SettingItem setting)
	{
		var entry = new Entry
		{
			Text = setting.Value?.ToString(),
			Placeholder = setting.Description,

		};
		entry.TextChanged += (s, e) => setting.ValueChanged(e.NewTextValue);
		return entry;
	}

	private Entry CreateNumericEntry(SettingItem setting)
	{
		var entry = new Entry
		{
			Text = setting.Value?.ToString(),
			Placeholder = setting.Description,
			Keyboard = Keyboard.Numeric
		};
		entry.TextChanged += (s, e) =>
		{
			if (double.TryParse(e.NewTextValue, out double value))
			{
				setting.ValueChanged(value);
			}
		};
		return entry;
	}
}