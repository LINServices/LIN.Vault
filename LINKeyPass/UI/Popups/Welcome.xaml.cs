using LIN.UI.Views;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace LIN.UI.Popups;

public partial class Welcome : ContentPage
{

    PasskeyIntentDataModel modelo;

    public Welcome(PasskeyIntentDataModel modelo)
    {
        InitializeComponent();
        this.modelo = modelo;
        lbHora.Text = modelo.Hora.ToString();
        displayAppName.Text = " " + modelo.ApplicationName;
    }


    /// <summary>
    /// Acepta
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_Clicked(object sender, EventArgs e)
    {
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
                    modelo.Status = PassKeyStatus.Success;
                    modelo.Token = Sesion.Instance.Token;
                    AppShell.PassKeyHub.SendStatus(modelo);
                    this.Close();
                    return;
                }
                catch
                {
                }
            }
            else
            {
                await DisplayAlert("Invalido", "Inténtalo de nuevo", "Ok");
            }

        }
        else
        {

             await DisplayAlert("Biometría desactivada", "Este dispositivo no cuenta con sensores de biometría o se encuentran desactivados.", "Ok");


        }
    }


    /// <summary>
    /// No acepta
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Clicked_1(object sender, EventArgs e)
    {
        try
        {

            modelo.Status = PassKeyStatus.Rejected;
            modelo.Token = "";

            AppShell.PassKeyHub.SendStatus(modelo);
            this.Close();
        }
        catch
        {

        }
    }
}