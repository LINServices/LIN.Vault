using LIN.Access.Auth;
using LIN.Types.Responses;

namespace LIN.Authenticator.Services;

public class AuthService : IAuthService
{
    public bool IsSessionOpen => SessionAuth.IsOpen;

    public async Task<(bool Success, string Message)> LoginAsync(string user, string password)
    {
        var (session, response) = await SessionAuth.LoginWith(user, password);

        if (response == Responses.Success && session != null)
        {
            // Guardar en base de datos local
            LocalDataBase.Data.UserDB database = new();
            await database.SaveUser(new()
            {
                ID = session.Account.Id,
                UserU = session.Account.Identity.Unique,
                Password = password
            });
            return (true, "Success");
        }

        string errorMessage = response switch
        {
            Responses.InvalidPassword => "La contraseña es incorrecta",
            Responses.NotExistAccount => $"No se encontró el usuario '{user}'",
            Responses.UnauthorizedByOrg => "Tu organización no permite que accedas a esta app",
            _ => "Ocurrió un error inesperado. Inténtalo más tarde."
        };

        return (false, errorMessage);
    }

    public void Logout()
    {
        SessionAuth.CloseSession();
        LocalDataBase.Data.UserDB db = new();
        _ = db.DeleteUsers();
    }
}
