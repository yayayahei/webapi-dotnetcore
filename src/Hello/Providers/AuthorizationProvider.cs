using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Hello.Models;
using Hello.Repositories;
using Microsoft.AspNetCore.Authentication;

namespace Hello.Providers
{
    public class AuthorizationProvider : OpenIdConnectServerProvider
    {
        private readonly ClientRepository _clientRepository;
        private readonly UserRepository _userRepository;
        private readonly TokenRepository _tokenRepository;

        public AuthorizationProvider(ClientRepository clientRepository, UserRepository userRepository,
            TokenRepository tokenRepository)
        {
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }


        public override async Task ValidateTokenRequest(ValidateTokenRequestContext context)
        {
            // Reject token requests that don't use grant_type=password or grant_type=refresh_token.
            if (!context.Request.IsPasswordGrantType() && !context.Request.IsRefreshTokenGrantType())
            {
                context.Reject(error: OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    description: "Only grant_type=password and refresh_token requests are accepted by this server.");
                return;
            }

            var client = _clientRepository.GetClient(context.ClientId, context.ClientSecret);
            if (client != null)
            {
                context.Validate();
            }
        }

        public override async Task HandleTokenRequest(HandleTokenRequestContext context)
        {
            if (!context.Request.IsPasswordGrantType())
            {
                context.Reject(OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    "Only password grants are accepted by this server");
                return;
            }

            User existUser = await _userRepository.GetUser(context.Request.Username, context.Request.Password);
            if (existUser == null)
            {
                context.Reject(error: OpenIdConnectConstants.Errors.AccessDenied,
                    description: "Invalid user credentials.");
                return;
            }

            var identity = new ClaimsIdentity(context.Scheme.Name, OpenIdConnectConstants.Claims.Name,
                OpenIdConnectConstants.Claims.Role);
            identity.AddClaim(OpenIdConnectConstants.Claims.Subject, context.Request.Username);
            identity.AddClaim("urn:customclaim", "value", OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken);
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), new AuthenticationProperties(),
                context.Scheme.Name);
            ticket.SetScopes(OpenIdConnectConstants.Scopes.Profile);
            context.Validate(ticket);
        }

        public override async Task ValidateRevocationRequest(ValidateRevocationRequestContext context)
        {
            var client = _clientRepository.GetClient(context.ClientId, context.ClientSecret);
            if (client != null)
            {
                context.Validate();
            }
        }

        public override async Task HandleRevocationRequest(HandleRevocationRequestContext context)
        {
            _tokenRepository.RevokeToken(context.Request.Token);
            context.Revoked = true;
        }
    }
}