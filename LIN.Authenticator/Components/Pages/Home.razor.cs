using LIN.Authenticator.Services;
using LIN.Types.Cloud.Identity.Models;
using Microsoft.AspNetCore.Components;

namespace LIN.Authenticator.Components.Pages;

public partial class Home : IDisposable
{
    [Inject]
    public IBiometricService BiometricService { get; set; } = null!;

    [Inject]
    public IPassKeyService PassKeyService { get; set; } = null!;

    [Inject]
    public IAuthService AuthService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public OtpService OtpService { get; set; } = null!;

    [Inject]
    public OtpDataService OtpDataService { get; set; } = null!;

    /// <summary>
    /// Indicates if biometric authentication is available on this device.
    /// </summary>
    public bool IsBiometricAvailable { get; set; } = true;

    /// <summary>
    /// Reference to the child Drawer component.
    /// </summary>
    private Shared.Drawer? Drawer { get; set; }

    /// <summary>
    /// Number of passkeys used today.
    /// </summary>
    private int PassKeyCount { get; set; }

    /// <summary>
    /// List of pending passkey intents.
    /// </summary>
    private List<PassKeyModel> Intents { get; set; } = new();

    /// <summary>
    /// List of 2FA accounts.
    /// </summary>
    private List<Models.AccountModel> Accounts { get; set; } = new();

    private System.Timers.Timer? _otpTimer;


    protected override async Task OnInitializedAsync()
    {
        await LoadData();
        await SubscribeToEvents();
        StartOtpTimer();
    }


    /// <summary>
    /// Subscribes to real-time passkey events.
    /// </summary>
    private async Task SubscribeToEvents()
    {
        PassKeyService.OnReceiveIntent += HandleReceiveIntent;
        await PassKeyService.InitializeHubAsync();
    }


    /// <summary>
    /// Handles incoming passkey intents from the SignalR hub.
    /// </summary>
    private void HandleReceiveIntent(object? sender, PassKeyModel intent)
    {
        ShowOnDrawer(intent);
    }


    /// <summary>
    /// Loads initial data from the services.
    /// </summary>
    private async Task LoadData()
    {
        // Load intents count and list
        PassKeyCount = await PassKeyService.GetIntentsCountAsync();
        Intents = await PassKeyService.GetIntentsAsync();

        // Load 2FA Accounts
        Accounts = await OtpDataService.GetAccountsAsync();
        UpdateOtpCodes();

        // Check biometric availability
        IsBiometricAvailable = await BiometricService.IsAvailableAsync();

        if (Intents.Any())
        {
            ShowOnDrawer(Intents.First());
        }

        StateHasChanged();
    }


    private void StartOtpTimer()
    {
        _otpTimer = new System.Timers.Timer(1000);
        _otpTimer.Elapsed += (s, e) => UpdateOtpCodes();
        _otpTimer.AutoReset = true;
        _otpTimer.Enabled = true;
    }


    private void UpdateOtpCodes()
    {
        bool changed = false;
        foreach (var account in Accounts)
        {
            var newOtp = OtpService.GenerateOtp(account.Secret, account.Digits, account.Period);
            var remaining = OtpService.GetRemainingSeconds(account.Period);

            if (account.CurrentOtp != newOtp || account.RemainingSeconds != remaining)
            {
                account.CurrentOtp = newOtp;
                account.RemainingSeconds = remaining;
                changed = true;
            }
        }
        if (changed)
        {
            InvokeAsync(StateHasChanged);
        }
    }


    private void GoToScanner()
    {
        if (Application.Current?.MainPage?.Navigation != null)
        {
            Application.Current.MainPage.Navigation.PushAsync(new ScannerPage(OtpService, OtpDataService));
        }
    }


    private async Task DeleteAccount(Models.AccountModel account)
    {
        await OtpDataService.DeleteAccountAsync(account);
        Accounts.Remove(account);
        StateHasChanged();
    }


    /// <summary>
    /// Displays a passkey intent in the drawer for user action.
    /// </summary>
    private void ShowOnDrawer(PassKeyModel intent)
    {
        if (Drawer == null) return;

        Drawer.Passkey = intent;
        Drawer.Show();
    }


    /// <summary>
    /// Handles the successful completion of a passkey action.
    /// </summary>
    private void HandleSuccess()
    {
        PassKeyCount++;
        StateHasChanged();
    }


    /// <summary>
    /// Closes the current session and redirects to login.
    /// </summary>
    private async Task HandleLogout()
    {
        await InvokeAsync(() =>
        {
            PassKeyService.DisconnectHub();
            AuthService.Logout();
            NavigationManager.NavigateTo("/");
        });
    }


    public void Dispose()
    {
        PassKeyService.OnReceiveIntent -= HandleReceiveIntent;
        _otpTimer?.Dispose();
    }
}