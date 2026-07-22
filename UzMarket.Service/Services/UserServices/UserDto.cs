namespace UzMarket.ServiceLayer.Services.UserServices
{
    public class UserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Pinfl { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
        public string PassportSeries { get; set; } = null!;
    }
}
