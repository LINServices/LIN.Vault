using LIN.Services;

namespace LIN.UI.Views;


public partial class AppShell : Shell
{


    public static event EventHandler<PassKeyModel>? OnReciveIntent;


    /// <summary>
    /// Instancia de AppShell
    /// </summary>
    public static AppShell Instance { get; set; }







    /// <summary>
    ///  Pagina actual de contenido
    /// </summary>
    private static ContentPage? _actualPage;



    /// <summary>
    /// Obtiene o establece la pagina actual de contenido
    /// </summary>
    public static ContentPage? ActualPage
    {
        get => _actualPage; set
        {
            _actualPage = value;

        }
    }




   


    private void PassKeyHub_OnRecieveIntentAdmin(object? sender, PassKeyModel e)
    {
        this.Dispatcher.DispatchAsync(() =>
        {
            OnReciveIntent?.Invoke(this, e);
        });
    }



    





















    public static void SetTitle(string user)
    {
      
    }

    public static void SetImage(ImageSource source)
    {
       
    }





















    private static void Call(string response)
    {
        if (PhoneDialer.Default.IsSupported)
            PhoneDialer.Default.Open(response);
    }



}
