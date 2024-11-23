using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleImmichFrame.ImmichApi;
using SimpleImmichFrame.Models;

namespace SimpleImmichFrame.PageModels;

public partial class PhotoPageModel : ObservableObject
{
	private readonly PhotoManager manager;
	private readonly IServiceProvider provider;
	private readonly ISettingsService settings;
	private readonly INavigation navigation;



	[ObservableProperty]
	private DisplayPhoto? currentPhoto = null;

	[ObservableProperty]
	private string currentTime = DateTime.Now.ToString("HH:mm");

	private IDispatcherTimer currentTimeTimer;

	private IDispatcherTimer photoChangeTimer;

	public PhotoPageModel(PhotoManager manager, IServiceProvider provider, ISettingsService settings, INavigation nav)
	{
		currentTimeTimer = Application.Current.Dispatcher.CreateTimer();
		currentTimeTimer.Interval = TimeSpan.FromSeconds(1);
		currentTimeTimer.Tick += CurrentTimeTick;
		currentTimeTimer.Start();

		currentTimeTimer = Application.Current.Dispatcher.CreateTimer();
		currentTimeTimer.Interval = TimeSpan.FromSeconds(settings.Settings.ImageDuration);
		currentTimeTimer.Tick += NextPhotoTick;
		currentTimeTimer.Start();

		this.navigation = nav;

		this.manager = manager;
		this.provider = provider;
		this.settings = settings;
	}

	[RelayCommand]
	private async Task Appearing()
	{
		this.CurrentPhoto = await this.manager.GetNextPhoto();
	}

	[RelayCommand]
	public async Task NextPhoto()
	{
		this.CurrentPhoto = await this.manager.GetNextPhoto();
	}

	[RelayCommand]
	public async Task GoToSettings()
	{
		await this.navigation.PushAsync(provider.GetRequiredService<SettingsPage>());
		this.navigation.RemovePage(this.navigation.NavigationStack.First());
	}

	private void CurrentTimeTick(object sender, EventArgs e)
	{
		CurrentTime = DateTime.Now.ToString("HH:mm");
	}

	private void NextPhotoTick(object sender, EventArgs e)
	{
		_ = this.NextPhoto();
	}
}