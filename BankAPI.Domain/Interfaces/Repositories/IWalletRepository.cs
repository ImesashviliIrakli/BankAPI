using BankAPI.Domain.Entities.Bank;

namespace BankAPI.Domain.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        Task<List<Wallet>> GetAccounts(string userId);
        Task<List<Wallet>> GetAccountsByCurrency(string userId, string currency);
        Task<Wallet> GetAccountByNumber(string accountNumber, string currency);
        Task<List<Wallet>> CreateWallet(string userId);
    }
}
