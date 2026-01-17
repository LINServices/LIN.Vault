using LIN.Authenticator.Services;
using Microsoft.AspNetCore.Components;

namespace LIN.Authenticator.Components.Pages;

public partial class Login
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private IAuthService AuthService { get; set; } = null!;

    [Inject]
    private IPassKeyService PassKeyService { get; set; } = null!;

    /// <summary>
    /// User identifier.
    /// </summary>
    private string User { get; set; } = string.Empty;

    /// <summary>
    /// Password string.
    /// </summary>
    private string Password { get; set; } = string.Empty;

    /// <summary>
    /// Message displayed during loading.
    /// </summary>
    private string LogMessage { get; set; } = "Iniciando Sesión";

    /// <summary>
    /// Error message to display.
    /// </summary>
    private string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Visibility of the error label.
    /// </summary>
    private bool IsErrorVisible { get; set; } = false;

    /// <summary>
    /// Current UI section (0: Login form, 1: Success, 2: Failure, 3: Loading).
    /// </summary>
    private int Section { get; set; } = 3;


    protected override async Task OnInitializedAsync()
    {
        if (AuthService.IsSessionOpen)
        {
            NavigationManager.NavigateTo("/home");
            return;
        }

        LocalDataBase.Data.UserDB database = new();
        var user = await database.GetDefault();

        if (user == null)
        {
            UpdateSection(0);
            return;
        }

        UpdateSection(3);
        User = user.UserU;
        Password = user.Password;

        await HandleLogin();
    }


    /// <summary>
    /// Updates the current UI section and triggers a re-render.
    /// </summary>
    private void UpdateSection(int section)
    {
        InvokeAsync(() =>
        {
            Section = section;
            StateHasChanged();
        });
    }


    private void HideError()
    {
        IsErrorVisible = false;
        StateHasChanged();
    }


    private void GoToForgotPassword()
    {
        NavigationManager.NavigateTo("/login/forgetPassword");
    }


    private void ShowError(string message)
    {
        InvokeAsync(() =>
        {
            UpdateSection(0);
            IsErrorVisible = true;
            ErrorMessage = message;
            StateHasChanged();
        });
    }


    /// <summary>
    /// Handles the login process.
    /// </summary>
    private async Task HandleLogin()
    {
        UpdateSection(3);
        HideError();

        if (string.IsNullOrWhiteSpace(User) || string.IsNullOrWhiteSpace(Password))
        {
            ShowError("Completa todos los campos");
            return;
        }

        var (success, message) = await AuthService.LoginAsync(User, Password);

        if (success)
        {
            await PassKeyService.InitializeHubAsync();
            NavigationManager.NavigateTo("/home");
        }
        else
        {
            ShowError(message);
        }
    }
}