using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;

namespace SimpleImmichFrame;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
				.UseMauiApp<App>()
				.UseMauiCommunityToolkit()
				.ConfigureSyncfusionToolkit()
				.ConfigureMauiHandlers(handlers =>
				{
				})
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
					fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
					fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
				});

#if DEBUG
		builder.Logging.AddDebug();
		builder.Services.AddLogging(configure => configure.AddDebug());
#endif

		builder.Services.AddSingleton<ProjectRepository>();
		builder.Services.AddSingleton<TaskRepository>();
		builder.Services.AddSingleton<CategoryRepository>();
		builder.Services.AddSingleton<TagRepository>();
		builder.Services.AddSingleton<SeedDataService>();
		builder.Services.AddSingleton<ModalErrorHandler>();
		builder.Services.AddSingleton<MainPageModel>();
		builder.Services.AddSingleton<ProjectListPageModel>();
		builder.Services.AddSingleton<SettingsViewModel>();
		builder.Services.AddSingleton<ISettingsService, SettingsService>();
		builder.Services.AddSingleton<ManageMetaPageModel>();

		builder.Services.AddTransientWithShellRoute<ProjectDetailPage, ProjectDetailPageModel>("project");
		builder.Services.AddTransientWithShellRoute<TaskDetailPage, TaskDetailPageModel>("task");
		//builder.Services.AddSingletonWithShellRoute<SettingsPage, SettingsViewModel>("settings");
		ModifyUiTextField();
		return builder.Build();
	}


	internal static void ModifyUiTextField()
	{
		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
		{
#if __ANDROID__
			handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
			//handler.PlatformView.setb = UITextBorderStyle.None;

			//handler.PlatformView.Layer.BorderColor = UIColor.SystemGray3.CGColor;
			//handler.PlatformView.Layer.BorderWidth = 2.0f;
			//handler.PlatformView.Layer.MasksToBounds = true;
			//handler.PlatformView.Layer.CornerRadius = 8.0f;

			//handler.PlatformView.EditingDidBegin += (sender, args) =>
			//{
			//	handler.PlatformView.Layer.BorderColor = ((Color)App.Current.Resources["Primary"]).ToCGColor();
			//};

			//handler.PlatformView.EditingDidEnd += (sender, args) =>
			//{
			//	handler.PlatformView.Layer.BorderColor = UIColor.SystemGray3.CGColor;
			//};
#endif
		});
	}

}
