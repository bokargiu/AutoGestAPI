namespace AutoGestAPI.DTO_s
{
    public class UserLoginDTO
    {
        public string UserOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public UserLoginDTO(string UserOrEmail, string Password)
        {
            this.UserOrEmail = UserOrEmail;
            this.Password = Password;
        }
    }
}
