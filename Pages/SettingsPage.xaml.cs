namespace SimpleImmichFrame.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(IServiceProvider provider)
	{
		InitializeComponent();
		BindingContext = new SettingsViewModel(provider.GetRequiredService<ISettingsService>(), provider, Navigation);
	}
}