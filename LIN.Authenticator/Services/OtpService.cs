using LIN.Authenticator.Models;
using OtpNet;
using System.Web;

namespace LIN.Authenticator.Services;

public class OtpService
{
    public string GenerateOtp(string secret, int digits = 6, int period = 30)
    {
        try
        {
            var bytes = Base32Encoding.ToBytes(secret);
            var totp = new Totp(bytes, step: period, totpSize: digits);
            return totp.ComputeTotp();
        }
        catch
        {
            return "000000";
        }
    }

    public int GetRemainingSeconds(int period = 30)
    {
        return period - (int)(DateTime.UtcNow.Ticks / 10_000_000L % period);
    }

    public AccountModel? ParseQrCode(string qrContent)
    {
        try
        {
            if (!qrContent.StartsWith("otpauth://totp/"))
                return null;

            var uri = new Uri(qrContent);
            var query = HttpUtility.ParseQueryString(uri.Query);

            var label = uri.AbsolutePath.TrimStart('/');
            string issuer = query["issuer"] ?? string.Empty;
            string accountName = label;

            if (label.Contains(':'))
            {
                var parts = label.Split(':');
                if (string.IsNullOrEmpty(issuer))
                    issuer = parts[0];
                accountName = parts[1].Trim();
            }

            return new AccountModel
            {
                Issuer = issuer,
                AccountName = accountName,
                Secret = query["secret"] ?? string.Empty,
                Digits = int.TryParse(query["digits"], out int d) ? d : 6,
                Period = int.TryParse(query["period"], out int p) ? p : 30
            };
        }
        catch
        {
            return null;
        }
    }
}
