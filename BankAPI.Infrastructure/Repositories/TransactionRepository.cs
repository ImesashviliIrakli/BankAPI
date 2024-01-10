using BankAPI.Domain.Dtos;
using BankAPI.Domain.Entities.Bank;
using BankAPI.Domain.Interfaces.Repositories;
using BankAPI.Infrastructure.Data;
using System.Reflection.Metadata.Ecma335;

namespace BankAPI.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        protected ResponseDto _response;
        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
            _response = new();
        }

        public async Task<ResponseDto> Deposit(Transaction transaction)
        {
            try
            {
                // Register Transaction
                await _context.Transactions.AddAsync(transaction);

                // Update Wallet
                Wallet userWallet = _context.Wallets.FirstOrDefault(x => x.AccountNumber == transaction.AccountNumber && x.Currency == transaction.Currency);

                if (userWallet != null)
                {
                    userWallet.CurrentBalance += transaction.Amount;

                    _context.Wallets.Update(userWallet);

                    await _context.SaveChangesAsync();

                    _response.Result = userWallet;
                }

                _response.IsSuccess = false;
                _response.Message = "Could not find wallet";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        public async Task<ResponseDto> Withdraw(Transaction transaction)
        {
            try
            {
                // Get Wallet
                Wallet userWallet = _context.Wallets.FirstOrDefault(x => x.AccountNumber == transaction.AccountNumber && x.Currency == transaction.Currency);

                if (userWallet == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not find wallet";

                    return _response;
                }

                if (transaction.Amount > userWallet.CurrentBalance)
                {
                    _response.IsSuccess = false;
                    _response.Message = "You don't have enough funds";

                    return _response;
                }

                // Register Transaction
                await _context.Transactions.AddAsync(transaction);

                // Update Wallet

                userWallet.CurrentBalance -= transaction.Amount;

                _context.Wallets.Update(userWallet);

                await _context.SaveChangesAsync();

                _response.Result = userWallet;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        public async Task<ResponseDto> TransferByAccountNumber(Transaction transaction)
        {
            try
            {
                // Get Main User Wallet
                Wallet mainUser = _context.Wallets.FirstOrDefault(x => x.AccountNumber == transaction.AccountNumber && x.Currency == transaction.Currency);

                Wallet remoteUser = _context.Wallets.FirstOrDefault(x => x.AccountNumber == transaction.RemoteAccountNumber && x.Currency == transaction.Currency);

                // Error Handling
                if (mainUser == null || remoteUser == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not find one of the wallets";

                    return _response;
                }


                if (transaction.Amount > mainUser.CurrentBalance)
                {
                    _response.IsSuccess = false;
                    _response.Message = "You don't have enough funds";

                    return _response;
                }

                // Register Transaction
                await _context.Transactions.AddAsync(transaction);

                // Transfer
                mainUser.CurrentBalance -= transaction.Amount;
                remoteUser.CurrentBalance += transaction.Amount;

                List<Wallet> wallets = new List<Wallet> { mainUser, remoteUser };

                _context.Wallets.UpdateRange(wallets);
                await _context.SaveChangesAsync();

                _response.Result = wallets;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}
