using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SimpleImmichFrame.ImmichApi;
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

		builder.Services.AddSingleton<ModalErrorHandler>();
		builder.Services.AddSingleton<SettingsViewModel>();
		builder.Services.AddSingleton<ISettingsService, SettingsService>();
		builder.Services.AddSingleton<ImmichClient>();
		builder.Services.AddSingleton<PhotoManager>();
		builder.Services.AddTransient<PhotoPage>();
		builder.Services.AddTransient<SettingsPage>();

		return builder.Build();
	}

}
