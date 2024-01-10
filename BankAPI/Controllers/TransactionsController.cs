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
        private readonly IMapper _mapper;
        protected ResponseDto _response;
        public TransactionsController(ITransactionRepository transaction, IMapper mapper)
        {
            _transaction = transaction;
            _mapper = mapper;
            _response = new();
        }

        [HttpPost("Deposit")]
        public async Task<IActionResult> Deposit(TransactionDto transactionDto)
        {
            Transaction transaction = _mapper.Map<Transaction>(transactionDto);

            transaction.RemoteAccountNumber = string.Empty;
            transaction.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            transaction.TransactionType = TransactionType.Deposit.ToString();

            _response = await _transaction.Deposit(transaction);

            return Ok(_response);
        }

        [HttpPost("Withdraw")]
        public async Task<IActionResult> Withdraw(TransactionDto transactionDto)
        {
            Transaction transaction = _mapper.Map<Transaction>(transactionDto);

            transaction.RemoteAccountNumber = string.Empty;
            transaction.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            transaction.TransactionType = TransactionType.Withdraw.ToString();

            _response = await _transaction.Withdraw(transaction);

            return Ok(_response);
        }

        [HttpPost("Transfer")]
        public async Task<IActionResult> Transfer(TransactionDto transactionDto)
        {
            Transaction transaction = _mapper.Map<Transaction>(transactionDto);

            transaction.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            transaction.TransactionType = TransactionType.Transfer.ToString();

            _response = await _transaction.TransferByAccountNumber(transaction);

            return Ok(_response);
        }
    }
}
