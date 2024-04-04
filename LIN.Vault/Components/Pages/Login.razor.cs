namespace LIN.Vault.Components.Pages;


public partial class Login
{

    /// <summary>
    /// Navegación.
    /// </summary>
    [Inject]
    private NavigationManager? NavigationManager { get; set; }


    /// <summary>
    /// Usuario
    /// </summary>
    private string User { get; set; } = "";


    /// <summary>
    /// Contraseña
    /// </summary>
    private string Password { get; set; } = "";


    /// <summary>
    /// Mensaje que se muestra al cargar
    /// </summary>
    private string LogMessage { get; set; } = "Iniciando Sesión";


    /// <summary>
    /// Mensaje de error
    /// </summary>
    private string ErrorMessage { get; set; } = "";




    private string errorVisible = "hidden";
    private bool cancelShow = false;

    private bool isLogin = false;
    private bool isAnimation = false;







    /// <summary>
    /// Evento al iniciar.




    protected override async Task OnInitializedAsync()
    {

        _= base.OnInitializedAsync();

        if (LIN.Access.Auth.SessionAuth.IsOpen)
        {
            NavigationManager?.NavigateTo("/home");
            return;
        }


        LocalDataBase.Data.UserDB database = new();

        // Usuario
        var user = await database.GetDefault();

        // Si no existe
        if (user == null)
            return;
        
        isLogin = true;
        StateHasChanged();
;
        User = user.UserU;
        Password = user.Password;

        Start();
      
    }





    /// <summary>
    /// Hace visibles los controles
    /// </summary>
    void ShowControls()
    {
        isLogin = false;
        StateHasChanged();
    }


    /// <summary>
    /// Oculta los controles
    /// </summary>
    void HideControls()
    {
        isLogin = true;
        StateHasChanged();
    }


    /// <summary>
    /// Oculta los errores
    /// </summary>
    void HideError()
    {
        errorVisible = "hidden";
        StateHasChanged();
    }



    /// <summary>
    /// Oculta los errores
    /// </summary>
    void GoToForget()
    {
        NavigationManager.NavigateTo("/login/forgetpassword");
    }


    /// <summary>
    /// Muestra un mensaje
    /// </summary>
    void ShowError(string message)
    {
        errorVisible = "visible";
        ErrorMessage = message;
        StateHasChanged();
    }



    /// <summary>
    /// Inicia sesión.
    /// </summary>
    async void Start()
    {

       
        HideControls();
        HideError();

        // Sin información
        if (User.Length <= 0 || Password.Length <= 0)
        {
            ShowControls();
            ShowError("Completa todos los campos");
            return;
        }

        // Inicio de sesión
        var login = await LIN.Access.Auth.SessionAuth.LoginWith(User, Password);

        if (login.Response == Responses.Success)
        {

            Home.PassKeyHub = Home.BuildHub();
            LocalDataBase.Data.UserDB database = new();

            await database.SaveUser(new() { ID = login.Sesion!.Account.Id, UserU = login.Sesion!.Account.Identity.Unique, Password = Password });


            //Online.StaticHub.LoadHub();
            NavigationManager?.NavigateTo("/home");
            return;

        }
        else if (login.Response == Responses.InvalidPassword)
        {
            ShowControls();
            ShowError("La contraseña es incorrecta");
        }
        else if (login.Response == Responses.NotExistAccount)
        {
            ShowControls();
            ShowError($"No existe el usuario {User}");
        }
        else if (login.Response == Responses.UnauthorizedByOrg)
        {
            ShowControls();
            ShowError($"Tu organización no permite que accedas a esta app");
        }
        else
        {
            ShowControls();
            ShowError("Inténtalo mas tarde");
        }


    }






    void CancelHi()
    {
        ShowControls();
        return;
    }




}