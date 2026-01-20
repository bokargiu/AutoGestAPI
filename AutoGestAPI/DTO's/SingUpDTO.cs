namespace AutoGestAPI.DTO_s
{
    public class SingUpDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public SingUpDTO(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }
    }
}
