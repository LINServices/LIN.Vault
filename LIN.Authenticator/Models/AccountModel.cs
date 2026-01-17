using SQLite;

namespace LIN.Authenticator.Models;

public class AccountModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Issuer { get; set; } = string.Empty;

    public string AccountName { get; set; } = string.Empty;

    public string Secret { get; set; } = string.Empty;

    public int Digits { get; set; } = 6;

    public int Period { get; set; } = 30;

    [Ignore]
    public string CurrentOtp { get; set; } = string.Empty;

    [Ignore]
    public double RemainingSeconds { get; set; }
}
