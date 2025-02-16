#if ANDROID
using Android.Views;
using CommunityToolkit.Maui;

#endif
using Microsoft.Extensions.Logging;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using LIN.Access.Auth;
using Microsoft.Extensions.Configuration;

namespace LIN.Vault;

public static class MauiProgram
{

    /// <summary>
    /// Crear app Maui.
    /// </summary>
    public static MauiApp CreateMauiApp()
    {
        var builder = Microsoft.Maui.Hosting.MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            })
            .UseMauiCommunityToolkit()
            .UseBarcodeReader();

        var config = Configuracion.LoadConfiguration();
        builder.Configuration.AddConfiguration(config);

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddAuthenticationService(app: config["app:key"]);

        // Lector de huellas.
        builder.Services.AddSingleton(typeof(IFingerprint), CrossFingerprint.Current);

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }



    /// <summary>
    /// En Android, establecer el color de la barra ne navegación al color de la app.
    /// </summary>
    public static void SetUIColors()
    {
#if ANDROID
        var currentActivity = Platform.CurrentActivity;

        if (currentActivity == null || currentActivity.Window == null)
            return;

        var currentTheme = AppInfo.RequestedTheme;

        if (currentTheme == AppTheme.Light)
        {
            currentActivity.Window.SetStatusBarColor(new(255, 255, 255));
            currentActivity.Window.SetNavigationBarColor(new(255, 255, 255));
            currentActivity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
        }
        else
        {
            currentActivity.Window.SetStatusBarColor(new(0, 0, 0));
            currentActivity.Window.SetNavigationBarColor(new(0, 0, 0));
            currentActivity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.Visible;
        }
#endif
    }


}