#if ANDROID
using Android.Views;
#endif
using CommunityToolkit.Maui;
using LIN.Access.Auth;
using LIN.Authenticator.Services;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

namespace LIN.Authenticator
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            }).UseMauiCommunityToolkit().UseBarcodeReader();
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<IBiometricService, BiometricService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IPassKeyService, PassKeyService>();
            builder.Services.AddSingleton<OtpService>();
            builder.Services.AddSingleton<OtpDataService>();

            var config = Configuracion.LoadConfiguration();
            builder.Services.AddAuthenticationService(app: config["app:key"]);

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
}