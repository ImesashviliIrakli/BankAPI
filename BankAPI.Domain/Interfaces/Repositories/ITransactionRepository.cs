using BankAPI.Domain.Dtos;
using BankAPI.Domain.Entities.Bank;
using BankAPI.Domain.Enums;

namespace BankAPI.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task<ResponseDto> TransferByAccountNumber(Transaction transaction);
        Task<ResponseDto> ProcessTransaction(Transaction transaction, TransactionType transactionType);
    }
}
