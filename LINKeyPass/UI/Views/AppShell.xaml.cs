using LIN.Services;

namespace LIN.UI.Views;


public partial class AppShell : Shell
{


    public static event EventHandler<PassKeyModel>? OnReciveIntent;


    /// <summary>
    /// Instancia de AppShell
    /// </summary>
    public static AppShell Instance { get; set; }



    /// <summary>
    /// Hub de Cuenta
    /// </summary>
    public static readonly Access.Auth.Hubs.AccountHub Hub = new(BuildHub());



    /// <summary>
    /// Hub de Cuenta
    /// </summary>
    public static readonly Access.Auth.Hubs.PassKeyHub PassKeyHub = new(LIN.Access.Auth.Session.Instance.Account.Usuario, true);



    private static async Task<DeviceModel> BuildHub()
    {




        var model = new DeviceModel()
        {
            Name = MauiProgram.GetDeviceName(),
            Cuenta = LIN.Access.Auth. Session.Instance.Account.ID,
            Modelo = DeviceInfo.Current.Model,
            BateryConected = BatteryService.IsChargin,
            BateryLevel = BatteryService.Percent,
            Manufacter = DeviceInfo.Current.Manufacturer,
            OsVersion = DeviceInfo.Current.VersionString,
            Platform = MauiProgram.GetPlatform(),
            Token = LIN.Access.Auth.Session.Instance.AccountToken,
            App = Applications.Admin
        };



        var location = await LocationService.GetLocation();

        double Logitud = location.Longitude;
        double Latitud = location.Latitude;

        model.Logitud = Logitud;
        model.Latitud = Latitud;

        return model;

    }







    /// <summary>
    ///  Pagina actual de contenido
    /// </summary>
    private static ContentPage? _actualPage;



    /// <summary>
    /// Obtiene o establece la pagina actual de contenido
    /// </summary>
    public static ContentPage? ActualPage
    {
        get => _actualPage; set
        {
            _actualPage = value;

        }
    }




    /// <summary>
    /// Constructor
    /// </summary>
    public AppShell()
    {
        InitializeComponent();
        Instance = this;
        A();

        // Eventos d

        BatteryService.StatusChange += BatteryService_StatusChange;

    }


    async void A()
    {
        await PassKeyHub.Suscribe();

        PassKeyHub.OnRecieveIntentAdmin += PassKeyHub_OnRecieveIntentAdmin;
    }

    private void PassKeyHub_OnRecieveIntentAdmin(object? sender, PassKeyModel e)
    {
        this.Dispatcher.DispatchAsync(() =>
        {
            OnReciveIntent?.Invoke(this, e);
        });
    }



    /// <summary>
    /// EVENTO: Cambia el estado
    /// </summary>
    private void BatteryService_StatusChange(object? sender, BatteryStatus e)
    {

        if (Hub.DeviceModel == null)
            return;

        if (Hub.DeviceModel.BateryLevel != e.Percent || Hub.DeviceModel.BateryConected != e.IsChargin)
        {
            Hub.DeviceModel.BateryLevel = e.Percent;
            Hub.DeviceModel.BateryConected = e.IsChargin;
        }
        Hub.SendBattery();
    }





















    public static void SetTitle(string user)
    {
        Instance.lbUser.Text = user;
    }

    public static void SetImage(ImageSource source)
    {
        Instance.perfil.Source = source;
    }





















    private static void Call(string response)
    {
        if (PhoneDialer.Default.IsSupported)
            PhoneDialer.Default.Open(response);
    }



}
