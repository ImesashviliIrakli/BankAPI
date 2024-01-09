using BankAPI.Domain.Dtos.AuthDtos;
using BankAPI.Domain.Entities.Auth;
using BankAPI.Domain.Interfaces.Repositories;
using BankAPI.Domain.Interfaces.Services;
using BankAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BankAPI.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenGenerator _jwt;
        private readonly IWalletRepository _wallet;

        public AuthService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IJwtTokenGenerator jwtTokenGenerator,
            IWalletRepository wallet)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _jwt = jwtTokenGenerator;
            _wallet = wallet;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwt.GenerateToken(user, roles);

            UserDto userDto = new()
            {
                Email = user.Email,
                ID = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PrivateNumber = user.PrivateNumber
            };

            LoginResponseDto loginResponseDto = new()
            {
                User = userDto,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                FirstName = registrationRequestDto.FirstName,
                LastName = registrationRequestDto.LastName,
                PhoneNumber = registrationRequestDto.PhoneNumber,
                PrivateNumber = registrationRequestDto.PrivateNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

                if (result.Succeeded)
                {
                    var userToReturn = _context.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);

                    // Create Wallet
                    bool createWallet = await _wallet.CreateWallet(userToReturn.Id);

                    if (!createWallet)
                    {
                        return "Could not create wallet";
                    }

                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        FirstName = userToReturn.FirstName,
                        LastName = userToReturn.LastName,
                        PhoneNumber = userToReturn.PhoneNumber,
                        PrivateNumber = userToReturn.PrivateNumber
                    };

                    return string.Empty;
                }

                return result.Errors.FirstOrDefault().Description;
            }
            catch (Exception)
            {
                return "Error Encountered";
            }
        }
    }
}
