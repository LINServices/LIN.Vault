using LIN.Authenticator.Services;
using LIN.Types.Cloud.Identity.Enumerations;
using LIN.Types.Cloud.Identity.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LIN.Authenticator.Components.Shared;

public partial class Drawer
{
    [Inject]
    private IBiometricService BiometricService { get; set; } = null!;

    [Inject]
    private IPassKeyService PassKeyService { get; set; } = null!;

    [Inject]
    private IJSRuntime JS { get; set; } = null!;

    /// <summary>
    /// Event triggered when the passkey is successfully authenticated.
    /// </summary>
    [Parameter]
    public Action OnAccept { get; set; } = () => { };

    /// <summary>
    /// Indicates if the authentication process was successful.
    /// </summary>
    private bool _isSuccess = false;

    /// <summary>
    /// Current passkey model being processed.
    /// </summary>
    private PassKeyModel? _passkey;

    public PassKeyModel? Passkey
    {
        get => _passkey;
        set
        {
            _passkey = value;
            _isSuccess = false;
            InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Shows the drawer using JS interop.
    /// </summary>
    public void Show()
    {
        JS.InvokeVoidAsync("ShowDrawer", "drawer-bottom-example", DotNetObjectReference.Create(this));
    }

    /// <summary>
    /// Hides the drawer using JS interop.
    /// </summary>
    public void Hide()
    {
        JS.InvokeVoidAsync("CloseDrawer", "drawer-bottom-example");
    }

    /// <summary>
    /// Handles the "Accept" action from the UI.
    /// </summary>
    private async void HandleAccept()
    {
        if (Passkey == null)
            return;

        // Check if biometric authentication is available.
        var isEnabled = await BiometricService.IsAvailableAsync();

        // Authenticate the user.
        bool authenticated = await BiometricService.AuthenticateAsync(
            isEnabled ? "Autenticación" : "LIN Passkey",
            isEnabled ? "Crear llave de acceso" : "Confirma tu identidad"
        );

        if (authenticated)
        {
            await HandleSuccessfulAuthentication();
            OnAccept();
        }
    }

    /// <summary>
    /// Updates the passkey status and notifies the hub.
    /// </summary>
    private async Task HandleSuccessfulAuthentication()
    {
        if (Passkey == null) return;

        Passkey.Status = PassKeyStatus.Success;
        Passkey.Token = LIN.Access.Auth.SessionAuth.Instance.AccountToken;

        PassKeyService.SendStatus(Passkey);

        _isSuccess = true;
        StateHasChanged();

        await Task.Delay(4000);
        Hide();
    }

    /// <summary>
    /// Handles the "Cancel" or "Close" action.
    /// </summary>
    private void HandleClose()
    {
        if (Passkey == null)
        {
            Hide();
            return;
        }

        try
        {
            Passkey.Status = PassKeyStatus.Rejected;
            Passkey.Token = string.Empty;
            PassKeyService.SendStatus(Passkey);
            Hide();
        }
        catch
        {
            Hide();
        }
    }
}
