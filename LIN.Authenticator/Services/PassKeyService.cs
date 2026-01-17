using LIN.Access.Auth.Hubs;
using LIN.Types.Cloud.Identity.Models;
using LIN.Types.Responses;

namespace LIN.Authenticator.Services;

public class PassKeyService : IPassKeyService
{
    private PassKeyHub? _hub;
    public event EventHandler<PassKeyModel>? OnReceiveIntent;

    public async Task InitializeHubAsync()
    {
        if (_hub != null) return;

        var session = LIN.Access.Auth.SessionAuth.Instance;
        _hub = new PassKeyHub(session.Account.Identity.Unique, string.Empty, session.AccountToken, true);

        _hub.OnReceiveIntent += (s, e) => OnReceiveIntent?.Invoke(this, e);
        await _hub.Suscribe();
    }

    public void DisconnectHub()
    {
        _hub?.Disconnect();
        _hub = null;
    }

    public async Task<List<PassKeyModel>> GetIntentsAsync()
    {
        var result = await LIN.Access.Auth.Controllers.Intents.ReadAll(LIN.Access.Auth.SessionAuth.Instance.AccountToken);
        return result.Response == Responses.Success ? new List<PassKeyModel>(result.Models) : new List<PassKeyModel>();
    }

    public async Task<int> GetIntentsCountAsync()
    {
        var result = await LIN.Access.Auth.Controllers.Intents.Count(LIN.Access.Auth.SessionAuth.Instance.AccountToken);
        return result.Model;
    }

    public void SendStatus(PassKeyModel passkey)
    {
        _hub?.SendStatus(passkey);
    }
}
