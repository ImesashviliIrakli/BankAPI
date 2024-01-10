using AutoMapper;
using BankAPI.Domain.Dtos;
using BankAPI.Domain.Dtos.BankDtos;
using BankAPI.Domain.Entities.Bank;
using BankAPI.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletRepository _wallet;
        private readonly IMapper _mapper;
        protected ResponseDto _response;
        public WalletController(IWalletRepository wallet, IMapper mapper)
        {
            _wallet = wallet;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet("GetAccounts")]
        public async Task<IActionResult> GetAccounts()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            List<Wallet> wallets = await _wallet.GetAccounts(userId);

            if (wallets.Count() == 0)
            {
                _response.IsSuccess = false;
                _response.Message = "Could not find wallets";

                return NotFound(_response);
            }

            IEnumerable<WalletDto> walletDto = _mapper.Map<IEnumerable<WalletDto>>(wallets);

            _response.Result = walletDto;

            return Ok(_response);
        }

        [HttpGet("GetAccounts/{currency}")]
        public async Task<IActionResult> GetAccountByCurrency(string currency)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Wallet wallet = await _wallet.GetAccountByCurrency(userId, currency);

            if (wallet == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Could not find wallet";

                return NotFound(_response);
            }

            WalletDto walletDto = _mapper.Map<WalletDto>(wallet);

            _response.Result = walletDto;

            return Ok(_response);
        }
    }
}
