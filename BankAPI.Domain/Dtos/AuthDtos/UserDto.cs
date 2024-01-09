namespace BankAPI.Domain.Dtos.AuthDtos
{
    public class UserDto
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PrivateNumber { get; set; }
    }
}
