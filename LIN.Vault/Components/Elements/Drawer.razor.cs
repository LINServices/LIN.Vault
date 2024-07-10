using LIN.Types.Cloud.Identity.Enumerations;
using LIN.Types.Cloud.Identity.Models;
using LIN.Vault.Components.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace LIN.Vault.Components.Elements;


public partial class Drawer
{

    /// <summary>
    /// Al aceptar.
    /// </summary>
    [Parameter]
    public Action OnAccept { get; set; } = () => { };


    private bool isOk = false;



    private PassKeyModel? passkey;


    public PassKeyModel? Passkey
    {
        get => passkey;
        set
        {
            passkey = value;
            isOk = false;
            InvokeAsync(() =>
            {

                StateHasChanged();
            });

        }
    }


    public void Show()
    {
        JS.InvokeVoidAsync("ShowDrawer", "drawer-bottom-example", DotNetObjectReference.Create(this), "btn-close-panel");
    }


    private async void OnSuccess()
    {

        if (Passkey == null)
            return;

        OnAccept();

        // Obtiene si hay sensor
        var isEnabled = await CrossFingerprint.Current.IsAvailableAsync();

        if (isEnabled)
        {

            // Respuesta
            var request = new AuthenticationRequestConfiguration("Autenticación", $"Crear llave de acceso");

            // Resultado
            var result = await CrossFingerprint.Current.AuthenticateAsync(request);

            // Autenticación correcta
            if (result.Authenticated)
            {
                try
                {
                    await SuccessAuth();
                    return;
                }
                catch
                {
                }
            }
            else
            {
                // await DisplayAlert("Invalido", "Inténtalo de nuevo", "Ok");
            }

        }
        else
        {

            // Autenticación con contraseña.
            var request = new AuthenticationRequestConfiguration
            ("LIN Passkey", "Confirma tu contraseña")
            {
                FallbackTitle = "Use PIN",
                AllowAlternativeAuthentication = true,
            };

            var result = await CrossFingerprint.Current.AuthenticateAsync(request);

            if (result.Authenticated)
            {
                await SuccessAuth();
            }
        }
    }



    private async Task SuccessAuth()
    {
        Passkey.Status = PassKeyStatus.Success;
        Passkey.Token = Access.Auth.SessionAuth.Instance.AccountToken;
        Home.PassKeyHub.SendStatus(Passkey);
        isOk = true;
        StateHasChanged();
        await Task.Delay(4000);

    }








    private void OnClose()
    {
        try
        {
            Passkey.Status = PassKeyStatus.Rejected;
            Passkey.Token = "";
            Home.PassKeyHub.SendStatus(Passkey);
        }
        catch
        {

        }
    }
}
