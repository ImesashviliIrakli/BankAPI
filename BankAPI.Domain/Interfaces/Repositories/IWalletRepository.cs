using BankAPI.Domain.Entities.Bank;

namespace BankAPI.Domain.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        Task<List<Wallet>> GetAccounts(string userId);
        Task<Wallet> GetAccountByCurrency(string userId, string currency);
        Task<bool> CreateWallet(string userId);
    }
}
