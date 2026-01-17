using Microsoft.Extensions.Configuration;

namespace LIN.Authenticator;

public static class Configuracion
{
    public static IConfigurationRoot LoadConfiguration()
    {
        using var stream = Task.Run(() => FileSystem.OpenAppPackageFileAsync("appsettings.json")).Result;
        using var reader = new StreamReader(stream);

        var builder = new ConfigurationBuilder();
        builder.AddJsonStream(stream);
        return builder.Build();
    }
}
