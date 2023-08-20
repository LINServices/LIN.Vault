namespace LIN.UI.Views;


public partial class Home : ContentPage
{

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
        AppShell.ActualPage = this;
        InitializeComponent();

        LoadUserData();
        Load();
        SuscribeToHub();

        AppShell.OnReciveIntent += AppShell_OnReciveIntent;

    }

    private void AppShell_OnReciveIntent(object? sender, PassKeyModel e)
    {
        new Popups.Welcome(e).Show();
    }


    private void SuscribeToHub()
    {
       // AppShell.Hub.OnReceiveNotification += Hub_OnReceiveNotification;
    }

    private void Hub_OnReceiveNotification(object? sender, string e)
    {
        this.Dispatcher.DispatchAsync(async () =>
        {
            await RefreshData();
        });
    }


    /// <summary>
    /// Pagina apareciendo
    /// </summary>
    private void AppearingEvent(object? sender, EventArgs e)
    {
        AppShell.ActualPage = this;
        try
        {
            AppShell.SetImage(ImageEncoder.Decode(Access.Auth. SessionAuth.Instance.Account.Perfil));
        }
        catch
        {
        }
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
            //lbInfo.Text = "No hay conexion";
            //lbInfo.Show();
            return;
        }

        // Carga el cache
        // Controles = LoadCache(Notificacions);

        // Carga los controles a la vista
        //        LoadControls(Controles);

        // Si no hay productos
        if (!Intentos.Any())
        {
            //  lbInfo.Text = "No hay nada que mostrar aqui";
        }

        Render();





        // Muestra el mensaje
        //  lbInfo.Show();

    }


    private void Render()
    {

        foreach (var e in Intentos)
        {
            new Popups.Welcome(e).Show();
        }

    }


    /// <summary>
    /// Rellena los datos desde la base de datos
    /// </summary>
    private async Task<bool> RefreshData()
    {

        //// Items
        //var items = await LIN.Access.Auth.Controllers..ReadAll(AppShell.Hub.ID, Sesion.Instance.Informacion.Usuario);

        //// Analisis de respuesta
        //if (items.Response != Shared.Responses.Responses.Success)
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
        AppShell.SetTitle(Access.Auth.SessionAuth.Instance.Account.Nombre);
        AppShell.SetImage(ImageEncoder.Decode(Access.Auth.SessionAuth.Instance.Account.Perfil));

        var session = Access.Auth.SessionAuth.Instance.Account;

        displayName.Text = session.Nombre;
        displayOrg.Text = "Sin organización";
        perfil1.Source = ImageEncoder.Decode(session.Perfil);


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