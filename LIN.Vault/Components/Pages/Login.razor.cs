﻿namespace LIN.Vault.Components.Pages;


public partial class Login
{

    /// <summary>
    /// Navegación.
    /// </summary>
    [Inject]
    private NavigationManager? NavigationManager { get; set; }


    /// <summary>
    /// Obtiene si se esta log con una llave de acceso
    /// </summary>
    private bool IsWithKey { get; set; } = false;


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

        IsWithKey = false;
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
    /// Muestra un mensaje
    /// </summary>
    void GotoLoginKey()
    {
        IsWithKey = !IsWithKey;
        StateHasChanged();
    }



    /// <summary>
    /// Inicia sesión.
    /// </summary>
    async void Start()
    {

        if (IsWithKey)
        {
            StartKey();
            return;
        }

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








    Access.Auth.Hubs.PassKeyHub? hub;

    /// <summary>
    /// Inicia sesión
    /// </summary>
    async void StartKey()
    {
        cancelShow = false;
        LogMessage = "Revisa tu dispositivo";
        HideControls();
        HideError();

        // Sin información
        if (User.Length <= 0)
        {
            ShowControls();
            ShowError("Usuario requerido");
            return;
        }


        // Suscribir al Hub
        hub = new(User, "Q333Q", "");

        await hub.Suscribe();
        var reive = false;

        hub.OnReceiveResponse += async (o, e) =>
        {
            await InvokeAsync(async () =>
            {
                reive = true;

                switch (e.Status)
                {
                    case PassKeyStatus.Success:
                        break;

                    case PassKeyStatus.Rejected:
                        ShowControls();
                        ShowError("Passkey rechazada");
                        return;

                    case PassKeyStatus.Expired:
                        ShowControls();
                        ShowError("La sesión expiro");
                        return;

                    case PassKeyStatus.BlockedByOrg:
                        ShowControls();
                        ShowError("Tu organización no permite que inicies en esta aplicación.");
                        return;

                    default:
                        ShowControls();
                        ShowError("Hubo un error en Passkey");
                        return;
                }


                isAnimation = true;
                cancelShow = false;
                LogMessage = "";
                StateHasChanged();
                await Task.Delay(5000);
                isAnimation = false;
                cancelShow = false;
                LogMessage = "Iniciando Sesión";
                StateHasChanged();

                // Inicio de sesión
                var login = await Access.Auth.SessionAuth.LoginWith(e.Token);

                if (login.Response == Responses.Success)
                {
                    //Online.StaticHub.LoadHub();
                    NavigationManager?.NavigateTo("/home");
                    return;

                }
                else if (login.Response == Responses.InvalidPassword)
                {
                    ShowControls();
                    ShowError("La sesión de passkey ha expirado");
                }
                else if (login.Response == Responses.NotExistAccount)
                {
                    ShowControls();
                    ShowError($"No existe el usuario {User}");
                }
                else
                {
                    ShowControls();
                    ShowError("Inténtalo de nuevo mas tarde");
                }
            });

            // Hace el inicio



        };

        var intent = new Types.Cloud.Identity.Models.PassKeyModel()
        {
            User = User
        };

        hub.SendIntent(intent);

        await Task.Delay(3000);
        cancelShow = true;
        StateHasChanged();

        await Task.Delay(60000);

        if (reive)
            return;

        hub.Disconnect();
        hub = null;
        ShowControls();
        ShowError("La sesión de passkey ha expirado");

    }




    void CancelHi()
    {
        hub?.Disconnect();
        hub = null;

        ShowControls();
        return;
    }




}