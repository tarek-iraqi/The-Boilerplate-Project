namespace Application.DTOs
{
    public class LoginResponseDTO
    {
        public LoginUserDataResponseDTO user { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }

    public class LoginUserDataResponseDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
    }
}
