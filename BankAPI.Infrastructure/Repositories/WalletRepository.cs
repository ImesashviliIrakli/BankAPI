using BankAPI.Domain.Entities.Bank;
using BankAPI.Domain.Interfaces.Repositories;
using BankAPI.Infrastructure.Data;

namespace BankAPI.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _context;
        public WalletRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateWallet(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return false;
                }

                Wallet wallet = new()
                {
                    UserId = userId,
                    AccountNumber = Guid.NewGuid().ToString()
                };

                await _context.Wallets.AddAsync(wallet);
                await _context.SaveChangesAsync();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
