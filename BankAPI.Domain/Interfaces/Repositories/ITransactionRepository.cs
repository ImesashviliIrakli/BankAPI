using BankAPI.Domain.Dtos;
using BankAPI.Domain.Entities.Bank;

namespace BankAPI.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task<ResponseDto> Deposit(Transaction transaction);
        Task<ResponseDto> Withdraw(Transaction transaction);
        Task<ResponseDto> TransferByAccountNumber(Transaction transaction);
    }
}
