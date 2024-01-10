using AutoMapper;
using BankAPI.Domain.Dtos;
using BankAPI.Domain.Dtos.BankDtos;
using BankAPI.Domain.Entities.Bank;
using BankAPI.Domain.Enums;
using BankAPI.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transaction;
        private readonly IWalletRepository _wallet;
        private readonly IMapper _mapper;
        protected ResponseDto _response;
        public TransactionsController(ITransactionRepository transaction, IMapper mapper, IWalletRepository wallet)
        {
            _transaction = transaction;
            _wallet = wallet;
            _mapper = mapper;
            _response = new();
        }

        [HttpPost("Deposit")]
        public async Task<IActionResult> Deposit(DepositWithdrawDto dwDto)
        {
            return await ProcessTransaction(dwDto, TransactionType.Deposit);
        }

        [HttpPost("Withdraw")]
        public async Task<IActionResult> Withdraw(DepositWithdrawDto dwDto)
        {
            return await ProcessTransaction(dwDto, TransactionType.Withdraw);
        }

        [HttpPost("Transfer")]
        public async Task<IActionResult> Transfer(TransactionDto transactionDto)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Wallet userWallet = await _wallet.GetAccountByNumber(transactionDto.AccountNumber, transactionDto.Currency);

            // Error Handling
            if (userWallet == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Could not find wallet";

                return BadRequest(_response);
            }

            if (userWallet.AccountNumber != transactionDto.AccountNumber)
            {
                _response.IsSuccess = false;
                _response.Message = "You are trying to access an account number that does not belong to you";

                return BadRequest(_response);
            }

            Transaction transaction = _mapper.Map<Transaction>(transactionDto);

            transaction.UserId = userId;
            transaction.TransactionType = TransactionType.Transfer.ToString();

            _response = await _transaction.TransferByAccountNumber(transaction);

            return Ok(_response);
        }

        #region Process Transaction

        private async Task<IActionResult> ProcessTransaction(DepositWithdrawDto dwDto, TransactionType transactionType)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Transaction transaction = _mapper.Map<Transaction>(dwDto);

            transaction.UserId = userId;
            transaction.TransactionType = transactionType.ToString();

            return Ok(await _transaction.ProcessTransaction(transaction, transactionType));
        }

        #endregion
    }
}
