using System.Collections.Generic;

namespace Hello.Repositories
{
    public class TokenRepository
    {
        private static readonly List<string> revocationTokens = new List<string>();

        public void RevokeToken(string token)
        {
            revocationTokens.Add(token);
        }

        public bool IsRevoked(string token)
        {
            return revocationTokens.Contains(token);
        }
    }
}