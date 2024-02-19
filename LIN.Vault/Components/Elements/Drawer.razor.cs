using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;
using LIN.Vault.Components.Pages;

namespace LIN.Vault.Components.Elements;


public partial class Drawer
{



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
        JS.InvokeVoidAsync("ShowDrawer", "drawer-bottom-example", "btn-close-panel");
    }


    private async void OnSuccess()
    {

        if (Passkey == null)
            return;

        // Obtiene si hay sensor
        var isEnabled = await CrossFingerprint.Current.IsAvailableAsync();

        if (isEnabled)
        {

            // Respuesta
            var request = new AuthenticationRequestConfiguration("Autenticación", $"Crear llave de acceso");

            // Resultado
            var result = await CrossFingerprint.Current.AuthenticateAsync(request);


            // Autenticacion correcta
            if (result.Authenticated)
            {
                try
                {
                    Passkey.Status = PassKeyStatus.Success;
                    Passkey.Token = Access.Auth.SessionAuth.Instance.AccountToken;
                    Home.PassKeyHub.SendStatus(Passkey);

                    isOk = true;
                    StateHasChanged();
                    await Task.Delay(4000);

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

           // await DisplayAlert("Biometría desactivada", "Este dispositivo no cuenta con sensores de biometría o se encuentran desactivados.", "Ok");


        }
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
