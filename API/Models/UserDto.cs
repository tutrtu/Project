namespace API.Models
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
