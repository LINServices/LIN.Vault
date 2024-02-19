using Microsoft.Extensions.Logging;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace LIN.Vault;

public static class MauiProgram
{

    /// <summary>
    /// Nueva app Maui.
    /// </summary>
    public static MauiApp CreateMauiApp()
    {

        // Constructor.
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Servicio blazor.
        builder.Services.AddMauiBlazorWebView();

        // Lector de huellas.
        builder.Services.AddSingleton(typeof(IFingerprint), CrossFingerprint.Current);

        // Debug mode.
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // Establecer app.
        LIN.Access.Auth.Build.SetAuth("DEFAULT");

        // Servicios de acceso.
        LIN.Access.Auth.Build.Init();

        return builder.Build();
    }


}