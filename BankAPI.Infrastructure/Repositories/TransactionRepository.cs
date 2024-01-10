using AutoMapper;
using BankAPI.Domain.Dtos;
using BankAPI.Domain.Dtos.BankDtos;
using BankAPI.Domain.Entities.Bank;
using BankAPI.Domain.Enums;
using BankAPI.Domain.Interfaces.Repositories;
using BankAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        protected ResponseDto _response;
        public TransactionRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new();
        }

        #region Process Transaction
        public async Task<ResponseDto> ProcessTransaction(Transaction transaction, TransactionType transactionType)
        {
            try
            {
                // Register Transaction
                await _context.Transactions.AddAsync(transaction);

                ResponseDto resposnse = new();

                // Main Logic
                switch (transactionType)
                {
                    case TransactionType.Deposit:
                        {
                            await Deposit(transaction);

                            return _response;
                        }
                    case TransactionType.Withdraw:
                        {
                            await Withdraw(transaction);

                            return _response;
                        }
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
        private async Task Deposit(Transaction transaction)
        {
            try
            {
                // Update Wallet
                Wallet userWallet = await _context.Wallets.FirstOrDefaultAsync(x => x.AccountNumber == transaction.AccountNumber && x.Currency == transaction.Currency);

                if (userWallet == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not find wallet";

                    return;
                }

                userWallet.CurrentBalance += transaction.Amount;

                _context.Wallets.Update(userWallet);

                await _context.SaveChangesAsync();

                _response.Result = userWallet;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
        }
        private async Task Withdraw(Transaction transaction)
        {
            try
            {
                // Get Wallet
                Wallet userWallet = await _context.Wallets.FirstOrDefaultAsync(x => x.AccountNumber == transaction.AccountNumber && x.Currency == transaction.Currency);

                if (userWallet == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not find wallet";

                    return;
                }

                if (transaction.Amount > userWallet.CurrentBalance)
                {
                    _response.IsSuccess = false;
                    _response.Message = "You don't have enough funds";

                    return;
                }

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
        }

        #endregion

        public async Task<ResponseDto> TransferByAccountNumber(Transaction transaction)
        {
            try
            {
                // Get Main User Wallet
                Wallet mainUser = await _context.Wallets.FirstOrDefaultAsync(x => x.AccountNumber == transaction.AccountNumber && x.Currency == transaction.Currency);
                Wallet remoteUser = await _context.Wallets.FirstOrDefaultAsync(x => x.AccountNumber == transaction.RemoteAccountNumber && x.Currency == transaction.Currency);

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

                List<Wallet> walletsToUpdate = new List<Wallet> { mainUser, remoteUser };

                _context.Wallets.UpdateRange(walletsToUpdate);
                await _context.SaveChangesAsync();

                List<WalletDto> walletDtos = _mapper.Map<List<WalletDto>>(walletsToUpdate);

                _response.Result = walletDtos;
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
