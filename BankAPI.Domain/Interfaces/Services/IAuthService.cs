using BankAPI.Domain.Dtos.AuthDtos;

namespace BankAPI.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
