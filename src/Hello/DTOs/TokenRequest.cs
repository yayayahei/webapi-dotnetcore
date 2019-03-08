using System.Collections.Generic;
using System.Net.Http;

namespace Hello.DTOs
{
    public class TokenRequest
    {
        public TokenRequest(string username, string password, string clientId, string clientSecret)
        {
            this.client_id = clientId;
            this.client_secret = clientSecret;
            this.grant_type = "password";
            this.password = password;
            this.username = username;
        }

        public FormUrlEncodedContent ToFormUrlContent()
        {
            return new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", this.grant_type),
                new KeyValuePair<string, string>("username", this.username),
                new KeyValuePair<string, string>("password", this.password),
                new KeyValuePair<string, string>("client_id", this.client_id),
                new KeyValuePair<string, string>("client_secret", this.client_secret),
            });
        }

        public string grant_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
}