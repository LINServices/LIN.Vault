namespace LIN.Vault
{
    public partial class MainPage : ContentPage
    {

        /// <summary>
        /// Nueva mainPage.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            // Al cambiar el tema.
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                MauiProgram.SetUIColors();
            };


        }



        /// <summary>
        /// Evento al abrir la app.
        /// </summary>
        protected override void OnAppearing()
        {
            // Establecer colores.
            MauiProgram.SetUIColors();
            base.OnAppearing();
        }


    }
}
