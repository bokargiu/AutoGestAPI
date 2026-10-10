namespace AutoGestAPI.DTO_s
{
    public class UserLoginDTO
    {
        public string NameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public UserLoginDTO(string NameOrEmail, string Password)
        {
            this.NameOrEmail = NameOrEmail;
            this.Password = Password;
        }
    }
}
