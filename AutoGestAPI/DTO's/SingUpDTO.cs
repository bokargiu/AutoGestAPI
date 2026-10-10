namespace AutoGestAPI.DTO_s
{
    public class SingUpDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public SingUpDTO(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
