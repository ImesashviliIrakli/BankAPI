using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.Domain.Entities.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [StringLength(11, MinimumLength = 11)]
        public string PrivateNumber { get; set; }
    }
}
