using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Hello.Repositories;

namespace Hello.Events
{
    public class ValidationEvents : OAuthValidationEvents
    {
        private readonly TokenRepository _tokenRepository;

        public ValidationEvents(TokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
            OnDecryptToken = context =>
            {
                if (_tokenRepository.IsRevoked(context.Token))
                {
                    context.Fail("error");
                }

                return Task.CompletedTask;
            };
        }
    }
}