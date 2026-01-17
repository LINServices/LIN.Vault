namespace LIN.Authenticator.Services;

public interface IAuthService
{
    bool IsSessionOpen { get; }
    Task<(bool Success, string Message)> LoginAsync(string user, string password);
    void Logout();
}
