namespace Hello.DTOs
{
    public class TokenResponse
    {
        public string scope { get; set; }
        public string token_type { get; set; }
        public string access_token { get; set; }
        public long expires_in { get; set; }
    }
}