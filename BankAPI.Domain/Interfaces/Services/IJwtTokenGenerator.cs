using BankAPI.Domain.Entities.Auth;

namespace BankAPI.Domain.Interfaces.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
