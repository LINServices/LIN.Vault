#if ANDROID
using Android.Views;
#endif


namespace LIN.UI.Views;


public partial class LoginLoading : ContentPage
{

    /// <summary>
    /// Base de datos local
    /// </summary>
    private readonly LocalDataBase.Data.UserDB Database;


    private string User = "";
    private string Pass = "";


    /// <summary>
    /// Constructor
    /// </summary>
    public LoginLoading(string user, string pass)
    {
        InitializeComponent();
        Database = new();
        Appearing += Login_Appearing;
        this.User = user;
        this.Pass = pass;
        TryGo();
    }





    /// <summary>
    /// Trata de inicia sesion con datos anteriores
    /// </summary>
    private async void TryGo()
    {
        await Task.Delay(200);
        await Sesion();

    }











    private void Login_Appearing(object? sender, EventArgs e)
    {
#if ANDROID
        var currentActivity = Platform.CurrentActivity;
        currentActivity.Window.SetStatusBarColor(new(247, 248, 253));
        currentActivity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
        //currentActivity.Window.SetTitleColor(new(0, 0, 0));
#endif
    }






    /// <summary>
    /// Click sobre boton iniciar sesion
    /// </summary>
    public async Task Sesion()
    {

        // Campos vacios
        if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Pass))
        {
            new Login("Por favor, asegúrate de llenar todos los campos requeridos.").ShowOnTop();
            this.Close();
            return;
        }

        // Connection a internet
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            new Login("No hay conexion a internet").ShowOnTop();
            this.Close();
            return;
        }


        try
        {

            Platforms platform = MauiProgram.GetPlatform();

            // Inicio de sesion
            var (Sesion, Response) = await LIN.Access.Auth.SessionAuth.LoginWith(User, Pass);


            // Evaluacion
            switch (Response)
            {
                // Ok
                case Responses.Success:
                    break;

                // No existe el usuario
                case Responses.NotExistAccount:
                    new Login("No existe este usuario").ShowOnTop();
                    this.Close();
                    return;

                // No existe el usuario
                case Responses.InvalidPassword:
                    new Login("Contrasena incorrecta").ShowOnTop();
                    this.Close();
                    return;

                // No existe el usuario
                case Responses.UnauthorizedByOrg:
                    new Login("Tu organización no permite que inicies en esta app").ShowOnTop();
                    this.Close();
                    return;

                // No existe el usuario
                case Responses.NotConnection:
                    new Login("No hay conexión").ShowOnTop();
                    this.Close();
                    return;

                // Hubo un error grave
                default:
                    new Login("Intentalo mas tarde").ShowOnTop();
                    this.Close();
                    return;

            }


            var modelo = Sesion.Account;
            modelo.Contraseña = Pass;

            await Database.SaveUser(new() { ID = modelo.ID, UserU = modelo.Identity.Unique, Password = Pass});

            // Abre la nueva ventana
            App.Current!.MainPage = new AppShell();
            this.Close();

        }
        catch (Exception ex)
        {
#if DEBUG
            await DisplayAlert("Error", ex.Source + "\n" + ex.Message + "\n" + ex.StackTrace, "OK");
#endif
            new Login("Hubo un error").ShowOnTop();
            this.Close();
            return;
        }

    }








}