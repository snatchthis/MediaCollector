using Microsoft.Extensions.Logging;
using MediaCollector.Data;
using MediaCollector.ViewModels;
using MediaCollector.Services;

namespace MediaCollector;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<FileCopyService>();
		builder.Services.AddSingleton<OperationSettings>();
		builder.Services.AddSingleton<InputFormViewModel>();
		builder.Services.AddSingleton<FileNameParser>();
		builder.Services.AddSingleton<ExtractService>();
		builder.Services.AddSingleton<IFilePickerService, TarFilePickerService>();

#if MACCATALYST
		builder.Services.AddTransient<IFolderPicker, MediaCollector.Platforms.MacCatalyst.FolderPicker>();
#endif
#if WINDOWS
		builder.Services.AddSingleton<IFolderPicker, MediaCollector.Platforms.Windows.FolderPicker>();
#endif

		return builder.Build();
	}
}

