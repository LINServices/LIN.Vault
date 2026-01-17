namespace LIN.Authenticator.Services;

public interface IBiometricService
{
    Task<bool> IsAvailableAsync();
    Task<bool> AuthenticateAsync(string title, string reason);
}
