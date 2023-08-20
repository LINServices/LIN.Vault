global using CommunityToolkit.Maui;
global using LIN.Types.Auth.Enumerations;
global using LIN.Controls;
global using LIN.Types.Enumerations;
global using LIN.Types.Responses;
global using LIN.Types.Auth.Models;
global using LIN.Types.Auth;
using LIN.UI.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using static LIN.Controls.Builder;


namespace LIN;


public static class MauiProgram
{




    /// <summary>
    /// Abre una nueva pagina
    /// </summary>
    public static void ShowOnTop(this ContentPage newPage)
    {
        var npage = new NavigationPage(newPage);
        NavigationPage.SetHasNavigationBar(newPage, false);
        App.Current!.MainPage = npage;
    }









    public static string GetDeviceName()
    {
        return DeviceInfo.Current.Name;
    }


    public static Platforms GetPlatform()
    {
        return Platforms.Android;
    }


    public static MauiApp CreateMauiApp()
    {

        // Builder
        var builder = MauiApp.CreateBuilder();

        // Configuracion
        builder
            .UseMauiApp<App>()
            .UseCustomControls()
            .ConfigureFonts(SetFonts)
            .UseMauiCommunityToolkit()
            .ConfigureEssentials(essentials =>
            {
                essentials.UseMapServiceToken("gCUbfMPXmCnDH2WR6uPk~JduHoZNxfxpNPxPihSH2aw~AoCRe2_PQIXYtX5u3x9BV03jFM3RE0zir7_M0c6laIIfdlNdgYeFhmohu_6bIQIp");
            })
            .ConfigureLifecycleEvents(events =>
            {
#if ANDROID
                events.AddAndroid(android => android
                    .OnActivityResult((activity, requestCode, resultCode, data) =>
                    {
                    })

                    .OnStart((activity) =>
                    {

                        // Battery.Default.BatteryInfoChanged += Default_BatteryInfoChanged;

                        if (LIN.Access.Auth.SessionAuth.IsOpen)
                            AppShell.Hub.Reconnect();

                    })

                    .OnCreate((activity, bundle) =>
                    {
                    })

                    .OnBackPressed((activity) =>
                    {
                        return false;
                    })

                    .OnRestart((act) =>
                    {
                    })

                       .OnStop((activity) =>
                       {
                           try
                           {
                               AppShell.Hub.CloseSesion();
                           }
                           catch
                           {

                           }

                       }
                       ));
#endif



                static bool LogEvent(string eventName, string type = null)
                {
                    System.Diagnostics.Debug.WriteLine($"Lifecycle event: {eventName}{(type == null ? string.Empty : $" ({type})")}");
                    return true;
                }
            });


        //builder.Services.AddSingleton<LIN.UI.Views.AppShell>();
        //builder.Services.AddSingleton<UI.Views.Home>();
        //builder.Services.AddSingleton<UI.Views.AccountPage>();
        //builder.Services.AddSingleton<LIN.LocalDataBase.Data.UserDB>();

        builder.Services.AddSingleton(typeof(IFingerprint), CrossFingerprint.Current);

        builder.Logging.AddDebug();

        // Servicios de LIN
        Services.BatteryService.Initialize();

        return builder.Build();
    }





    /// <summary>
    /// Configuracion de las fuentes
    /// </summary>
    private static void SetFonts(IFontCollection fonts)
    {
#if WINDOWS
        SetFontsAndroid(fonts);
#elif ANDROID
        SetFontsAndroid(fonts);
#endif
    }


    /// <summary>
    /// Configuracion de las fuentes
    /// </summary>
    private static void SetFontsWindows(IFontCollection fonts)
    {
        // Fuentes de Windows
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

        // Fuentes de la aplicacion LIN
        fonts.AddFont("Quicksand-Bold.ttf", "QSB");
        fonts.AddFont("Quicksand-Light.ttf", "QSL");
        fonts.AddFont("Quicksand-Medium.ttf", "QSM");
        fonts.AddFont("Quicksand-Regular.ttf", "QSR");
        fonts.AddFont("Quicksand-SemiBold.ttf", "QSSB");

        // Fuentes de utilidades
        fonts.AddFont("BarcodeFont.ttf", "Barcode");
    }

    /// <summary>
    /// Configuracion de las fuentes
    /// </summary>
    private static void SetFontsAndroid(IFontCollection fonts)
    {
        // Fuentes de Windows
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

        // Fuentes de la aplicacion LIN
        fonts.AddFont("Gilroy-Bold.ttf", "QSB");
        fonts.AddFont("Gilroy-Light.ttf", "QSL");
        fonts.AddFont("Gilroy-Medium.ttf", "QSM");
        fonts.AddFont("Gilroy-Regular.ttf", "QSR");
        fonts.AddFont("Gilroy-SemiBold.ttf", "QSSB");

        // Fuentes de utilidades
        fonts.AddFont("BarcodeFont.ttf", "Barcode");
    }

}
