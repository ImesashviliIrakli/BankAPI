using AutoMapper;
using BankAPI.Domain.Dtos.BankDtos;
using BankAPI.Domain.Entities.Bank;

namespace BankAPI.Domain
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Wallet, WalletDto>().ReverseMap();
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<DepositWithdrawDto, Transaction>();
        }
    }
}
