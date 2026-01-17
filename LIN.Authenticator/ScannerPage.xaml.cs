using LIN.Authenticator.Services;
using ZXing.Net.Maui;

namespace LIN.Authenticator;

public partial class ScannerPage : ContentPage
{
    private readonly OtpService _otpService;
    private readonly OtpDataService _otpDataService;
    private bool _isProcessing = false;

    public ScannerPage(OtpService otpService, OtpDataService otpDataService)
    {
        InitializeComponent();
        _otpService = otpService;
        _otpDataService = otpDataService;

        barcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.TwoDimensional,
            AutoRotate = true,
            Multiple = false
        };
    }

    private async void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (_isProcessing) return;

        var first = e.Results.FirstOrDefault();
        if (first == null) return;

        _isProcessing = true;

        var account = _otpService.ParseQrCode(first.Value);

        if (account != null)
        {
            await _otpDataService.SaveAccountAsync(account);
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Navigation.PopAsync();
            });
        }
        else
        {
            _isProcessing = false;
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
