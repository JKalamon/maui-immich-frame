using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

	[ObservableProperty]
	private string? exceptionText = null;

	[ObservableProperty]
	private bool showDeleteButton = false;

	private IDispatcherTimer currentTimeTimer;

	private IDispatcherTimer photoChangeTimer;

	public PhotoPageModel(PhotoManager manager, IServiceProvider provider, ISettingsService settings, INavigation nav)
	{
		if (Application.Current == null)
			throw new ApplicationException("Application.Current which is used to create timers is null.");

		currentTimeTimer = Application.Current.Dispatcher.CreateTimer();
		currentTimeTimer.Interval = TimeSpan.FromSeconds(1);
		currentTimeTimer.Tick += CurrentTimeTick;
		currentTimeTimer.Start();

		photoChangeTimer = Application.Current.Dispatcher.CreateTimer();
		photoChangeTimer.Interval = TimeSpan.FromSeconds(settings.Settings.ImageDuration);
		photoChangeTimer.Tick += NextPhotoTick;
		photoChangeTimer.Start();

		this.navigation = nav;

		this.manager = manager;
		this.provider = provider;
		this.settings = settings;
	}

	[RelayCommand]
	private async Task Appearing() => await GetNextPhotoInternal();

	[RelayCommand]
	public async Task NextPhoto() => await GetNextPhotoInternal();

	[RelayCommand]
	public async Task GoToSettings()
	{
		await this.navigation.PushAsync(provider.GetRequiredService<SettingsPage>());
		this.navigation.RemovePage(this.navigation.NavigationStack.First());
	}

	[RelayCommand]
	public async Task DeleteImage()
	{
		await this.manager.DeleteCurrentPhoto();
		await this.GetNextPhotoInternal();
	}

	[RelayCommand]
	public void ToggleDeleteButton()
	{
		this.ShowDeleteButton = !this.ShowDeleteButton;
	}

	private async Task GetNextPhotoInternal()
	{
		try
		{
			this.CurrentPhoto = await this.manager.GetNextPhoto();
			this.ShowDeleteButton = false;
			this.ExceptionText = null;
		}
		catch (Exception ex)
		{
			this.ExceptionText = ex.Message;
		}
	}

	private void CurrentTimeTick(object? sender, EventArgs e)
	{
		CurrentTime = DateTime.Now.ToString("HH:mm");
	}

	private void NextPhotoTick(object? sender, EventArgs e)
	{
		_ = this.NextPhoto();
	}
}