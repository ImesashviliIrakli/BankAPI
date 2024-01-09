namespace BankAPI.Domain.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        Task<bool> CreateWallet(string userId);
    }
}
