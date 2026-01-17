using LIN.Types.Cloud.Identity.Models;

namespace LIN.Authenticator.Services;

public interface IPassKeyService
{
    event EventHandler<PassKeyModel> OnReceiveIntent;
    Task InitializeHubAsync();
    void DisconnectHub();
    Task<List<PassKeyModel>> GetIntentsAsync();
    Task<int> GetIntentsCountAsync();
    void SendStatus(PassKeyModel passkey);
}
