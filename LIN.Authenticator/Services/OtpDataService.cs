using LIN.LocalDataBase;
using SQLite;

namespace LIN.Authenticator.Services;

public class OtpDataService
{
    private SQLiteAsyncConnection? _database;

    private async Task Init()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await _database.CreateTableAsync<Models.AccountModel>();
    }

    public async Task<List<Models.AccountModel>> GetAccountsAsync()
    {
        await Init();
        return await _database!.Table<Models.AccountModel>().ToListAsync();
    }

    public async Task<int> SaveAccountAsync(Models.AccountModel account)
    {
        await Init();
        if (account.Id != 0)
        {
            return await _database!.UpdateAsync(account);
        }
        else
        {
            return await _database!.InsertAsync(account);
        }
    }

    public async Task<int> DeleteAccountAsync(Models.AccountModel account)
    {
        await Init();
        return await _database!.DeleteAsync(account);
    }
}
