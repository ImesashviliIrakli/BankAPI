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

        public async Task<List<Wallet>> GetAccountsByCurrency(string userId, string currency)
        {
            try
            {
                List<Wallet> userWallet = await _context.Wallets.Where(x => x.UserId == userId && x.Currency == currency).ToListAsync();

                return userWallet;
            }
            catch (Exception)
            {
                return new List<Wallet>();
            }
        }

        public async Task<List<Wallet>> CreateWallet(string userId)
        {
            try
            {
                List<Wallet> newWalletList = new List<Wallet>();

                if (string.IsNullOrEmpty(userId))
                {
                    return newWalletList;
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

                    newWalletList.Add(newWallet);

                    await _context.Wallets.AddAsync(newWallet);
                }

                await _context.SaveChangesAsync();

                return newWalletList;
            }
            catch (Exception)
            {
                return new List<Wallet>();
            }
        }

        public async Task<Wallet> GetAccountByNumber(string accountNumber, string currency)
        {
            try
            {
                Wallet wallet = await _context.Wallets.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber && x.Currency == currency);

                return wallet;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
