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


    /// <summary>
    /// Visibilidad del error.
    /// </summary>
    private bool ErrorVisible { get; set; } = false;



    /// <summary>
    /// Sección actual.
    /// </summary>
    private int Section { get; set; } = 3;




    /// <summary>
    /// Constructor.
    /// </summary>
    public Login()
    {
    }



    /// <summary>
    /// Evento.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {


        if (Access.Auth.SessionAuth.IsOpen)
        {
            NavigationManager?.NavigateTo("/home");
            return;
        }

        _ = base.OnInitializedAsync();


        LocalDataBase.Data.UserDB database = new();

        // Usuario
        var user = await database.GetDefault();

        // Si no existe
        if (user == null)
        {
            UpdateSection(0);
            return;
        }


        UpdateSection(3);

        User = user.UserU;
        Password = user.Password;

        Start();

    }



    /// <summary>
    /// Actualizar la sección.
    /// </summary>
    /// <param name="section">Id de la sección</param>
    private void UpdateSection(int section)
    {
        InvokeAsync(() =>
        {
            Section = section;
            StateHasChanged();
        });
    }



    /// <summary>
    /// Oculta los errores
    /// </summary>
    void HideError()
    {
        ErrorVisible = false;
        StateHasChanged();
    }



    /// <summary>
    /// Oculta los errores
    /// </summary>
    void GoToForget()
    {
        NavigationManager?.NavigateTo("/login/forgetPassword");
    }



    /// <summary>
    /// Muestra un mensaje
    /// </summary>
    void ShowError(string message)
    {
        InvokeAsync(() =>
        {
            UpdateSection(0);
            ErrorVisible = true;
            ErrorMessage = message;
            StateHasChanged();
        });
    }




    /// <summary>
    /// Inicia sesión.
    /// </summary>
    private async void Start()
    {

       
        // Estado cargando.
        UpdateSection(3);

        // Ocultar el error.
        HideError();

        // Validar parámetros.
        if (User.Length <= 0 || Password.Length <= 0)
        {
            ShowError("Completa todos los campos");
            return;
        }


        // Iniciar sesión.
        var (Session, Response) = await LIN.Access.Auth.SessionAuth.LoginWith(User, Password);

        // Validar respuesta.
        switch (Response)
        {

            // Correcto.
            case Responses.Success:

                // Iniciar servicios de tiempo real.
                Home.PassKeyHub = Home.BuildHub();

                // Obtener local db.
                LocalDataBase.Data.UserDB database = new();

                // Guardar información.
                await database.SaveUser(new() { ID = Session!.Account.Id, UserU = Session!.Account.Identity.Unique, Password = Password });

                // Navegar.
                NavigationManager?.NavigateTo("/home");
                return;

            // Contraseña incorrecta.
            case Responses.InvalidPassword:
                ShowError("La contraseña es incorrecta");
                break;

            // No existe la cuenta.
            case Responses.NotExistAccount:
                ShowError($"No se encontró el usuario '{User}'");
                break;

            // Desautorizado por la organización.
            case Responses.UnauthorizedByOrg:
                ShowError($"Tu organización no permite que accedas a esta app");
                break;

            default:
                ShowError("Inténtalo mas tarde");
                break;


        }

    }



   



}