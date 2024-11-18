namespace SimpleImmichFrame.Pages;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}

	private async void OnSettingsClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///settings");
}