using Plugin.Fingerprint;

namespace LIN.UI.Views;


public partial class Home : ContentPage
{


    /// <summary>
    /// Hub de Cuenta
    /// </summary>
    public static readonly Access.Auth.Hubs.PassKeyHub PassKeyHub = new(LIN.Access.Auth.SessionAuth.Instance.Account.Identity.Unique, "",LIN.Access.Auth.SessionAuth.Instance.AccountToken , true);



    async void A()
    {
        await PassKeyHub.Suscribe();
        PassKeyHub.OnReceiveIntent += PassKeyHub_OnRecieveIntentAdmin;
    }





    private void PassKeyHub_OnRecieveIntentAdmin(object? sender, PassKeyModel e)
    {
        this.Dispatcher.DispatchAsync(() =>
        {
            // Abre la nueva ventana
            App.Current!.MainPage = new Popups.Welcome(e);
        });
    }




    /// <summary>
    /// Lista de intentos
    /// </summary>
    private List<PassKeyModel> Intentos = new();



    /// <summary>
    /// Constructor
    /// </summary>
    public Home()
    {
        Appearing += AppearingEvent;
        InitializeComponent();

        LoadFinger();

        LoadUserData();
        Load();
        A();


    }


    private async void LoadFinger()
    {


        var isEnabled = await CrossFingerprint.Current.IsAvailableAsync();



        if (isEnabled)
        {
            displayError.Hide();
            displayInfo.Show();
            displayPic.Source = ImageSource.FromFile("finger_il.png");
          
        }
        else
        {
            displayError.Show();
            displayInfo.Hide();
            displayPic.Source = ImageSource.FromFile("finger_il2.png");
        }

    }





    /// <summary>
    /// Pagina apareciendo
    /// </summary>
    private void AppearingEvent(object? sender, EventArgs e)
    {
        //AppShell.ActualPage = this;
        //try
        //{
        //    AppShell.SetImage(ImageEncoder.Decode(Access.Auth.SessionAuth.Instance.Account.Profile));
        //}
        //catch
        //{
        //}
    }



    /// <summary>
    /// Carga los datos
    /// </summary>
    private async void Load()
    {
        // Info
        ClearInterface();

        // Rellena los datos
        var dataRes = await RefreshData();

        // Comprueba si se rellenaron los datos
        if (!dataRes)
        {
            _ = DisplayAlert("Error", "Hubo un error al obtener informaci�n desde LIN Passkey", "OK");
            return;
        }

        Render();
    }


    /// <summary>
    /// Renderiza la lista de intentos
    /// </summary>
    private void Render()
    {

        foreach (var intento in Intentos)
            new Popups.Welcome(intento).Show();
        

    }


    /// <summary>
    /// Rellena los datos desde la base de datos
    /// </summary>
    private async Task<bool> RefreshData()
    {

        //// Items
        //var items = await LIN.Access.Auth.Controllers.Intents.ReadAll(LIN.Access.Auth.SessionAuth.Instance.AccountToken);

        //// An�lisis de respuesta
        //if (items.Response != Responses.Success)
        //    return false;

        //// Rellena los items
        //Intentos = items.Models.ToList();
        return true;

    }






    /// <summary>
    /// Limpia la interfaz
    /// </summary>
    private void ClearInterface()
    {
        //    lbInfo.Hide();
        //indicador.Hide();
        //content.Clear();
    }


































    /// <summary>
    /// Carga la informacion de usuario a la vista
    /// </summary>
    private async void LoadUserData()
    {
        //perfil.Source = ImageEncoder.Decode(Access.Auth.SessionAuth.Instance.Account.Perfil);


        //lbUser.Text = Access.Auth.SessionAuth.Instance.Account.Nombre;
        //AppShell.SetTitle(Access.Auth.SessionAuth.Instance.Account.Name);
        //AppShell.SetImage(ImageEncoder.Decode(Access.Auth.SessionAuth.Instance.Account.Profile));

        var session = Access.Auth.SessionAuth.Instance.Account;

        displayName.Text = session.Name;
        //displayOrg.Text = LIN.Access.Auth.SessionAuth.Instance.Account.OrganizationAccess?.Organization.Name ?? "Sin organizaci�n";
        perfil1.Source = ImageEncoder.Decode(session.Profile);


        //    lbUsuario.Text = "@" + Sesion.Instance.Informacion.UsuarioU;
        //    imgPerfil.Source = ;
        await Task.Delay(1);
    }




    //======== Eventos =========//



    /// <summary>
    /// Click sobre la imagen de perfil
    /// </summary>
    private void ImgPerfil_Clicked(object sender, EventArgs e)
    {
        GC.Collect();
    }


    /// <summary>
    /// Click sobre boton de inventarios
    /// </summary>
    private void BtnProductos_Clicked(object sender, EventArgs e)
    {

    }



    /// <summary>
    /// Boton de contactos
    /// </summary>
    private void BtnContactos_Clicked(object sender, EventArgs e)
    {
        GC.Collect();
        //new Contacts.Index().Show();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {

    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {


    }
}