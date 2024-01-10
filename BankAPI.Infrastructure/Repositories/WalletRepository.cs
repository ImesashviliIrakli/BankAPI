using BankAPI.Domain.Entities.Bank;
using BankAPI.Domain.Enums;
using BankAPI.Domain.Interfaces.Repositories;
using BankAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _context;
        public WalletRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Wallet>> GetAccounts(string userId)
        {
            try
            {
                List<Wallet> wallets = await _context.Wallets.Where(x => x.UserId == userId).ToListAsync();

                return wallets;
            }
            catch (Exception)
            {
                return new List<Wallet>();
            }
        }

        public async Task<Wallet> GetAccountByCurrency(string userId, string currency)
        {
            try
            {
                Wallet userWallet = await _context.Wallets.FirstOrDefaultAsync(x => x.UserId == userId && x.Currency == currency);

                return userWallet;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> CreateWallet(string userId)
        {
            try
            {
                List<Wallet> newWallets = new List<Wallet>();

                if (string.IsNullOrEmpty(userId))
                {
                    return false;
                }

                string accountNumber = Guid.NewGuid().ToString();

                foreach (Currency currency in Enum.GetValues(typeof(Currency)))
                {
                    Wallet newWallet = new()
                    {
                        UserId = userId,
                        AccountNumber = accountNumber,
                        Currency = currency.ToString()
                    };

                    await _context.Wallets.AddAsync(newWallet);
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
