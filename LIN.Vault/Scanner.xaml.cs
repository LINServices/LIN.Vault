namespace LIN.Vault;

public partial class Scanner : ContentPage
{
	public Scanner()
	{
		InitializeComponent();
	}


    private void scanner_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
       scanner.IsDetecting = false;
        var x = e.Results[0].Value;
        Navigation.RemovePage(this);
    }

}