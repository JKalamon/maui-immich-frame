namespace SimpleImmichFrame.Pages;

public partial class PhotoPage : ContentPage
{
	public PhotoPage(IServiceProvider provider)
	{
		InitializeComponent();
		BindingContext = new PhotoPageModel(
			provider.GetRequiredService<PhotoManager>(),
			provider,
			provider.GetRequiredService<ISettingsService>(),
			Navigation);
	}
}