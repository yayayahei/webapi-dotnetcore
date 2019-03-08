using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;

namespace Hello
{
    public class AuthorizationProvider : OpenIdConnectServerProvider
    {
//        private readonly dbContext _DbContext = new dbContext();
//        private readonly HashService _HashService = new HashService();

        public override async Task ValidateTokenRequest(ValidateTokenRequestContext context)
        {
            // Reject token requests that don't use grant_type=password or grant_type=refresh_token.
            if (!context.Request.IsPasswordGrantType() && !context.Request.IsRefreshTokenGrantType())
            {
                context.Reject(
                    error: OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    description: "Only grant_type=password and refresh_token " +
                                 "requests are accepted by this server.");

                return;
            }

            // Note: you can skip the request validation when the client_id
            // parameter is missing to support unauthenticated token requests.
            // if (string.IsNullOrEmpty(context.ClientId))
            // {
            //     context.Skip();
            // 
            //     return Task.CompletedTask;
            // }

            // Note: to mitigate brute force attacks, you SHOULD strongly consider applying
            // a key derivation function like PBKDF2 to slow down the secret validation process.
            // You SHOULD also consider using a time-constant comparer to prevent timing attacks.
            if (string.Equals(context.ClientId, "client_id", StringComparison.Ordinal) &&
                string.Equals(context.ClientSecret, "client_secret", StringComparison.Ordinal))
            {
                context.Validate();
            }

            // Note: if Validate() is not explicitly called,
            // the request is automatically rejected.
            return;
        }

        public override async Task HandleTokenRequest(HandleTokenRequestContext context)
        {
            if (!context.Request.IsPasswordGrantType())
            {
                context.Reject(OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    "Only password grants are accepted by this server");
                return;
            }

            if (!string.Equals(context.Request.Username, "Bob", StringComparison.Ordinal) ||
                !string.Equals(context.Request.Password, "P@ssw0rd", StringComparison.Ordinal))
            {
                context.Reject(
                    error: OpenIdConnectConstants.Errors.InvalidGrant,
                    description: "Invalid user credentials.");

                return;
            }

// Implement context.Request.Username/context.Request.Password validation here.
            // Note: you can call context Reject() to indicate that authentication failed.
            // Using password derivation and time-constant comparer is STRONGLY recommended.
            if (!string.Equals(context.Request.Username, "Bob", StringComparison.Ordinal) ||
                !string.Equals(context.Request.Password, "P@ssw0rd", StringComparison.Ordinal))
            {
                context.Reject(
                    error: OpenIdConnectConstants.Errors.InvalidGrant,
                    description: "Invalid user credentials.");

                return;
            }

            var identity = new ClaimsIdentity(context.Scheme.Name,
                OpenIdConnectConstants.Claims.Name,
                OpenIdConnectConstants.Claims.Role);

            // Add the mandatory subject/user identifier claim.
            identity.AddClaim(OpenIdConnectConstants.Claims.Subject, context.Request.Username);

            // By default, claims are not serialized in the access/identity tokens.
            // Use the overload taking a "destinations" parameter to make sure
            // your claims are correctly inserted in the appropriate tokens.
            identity.AddClaim("urn:customclaim", "value",
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken);

            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                context.Scheme.Name);

            // Call SetScopes with the list of scopes you want to grant
            // (specify offline_access to issue a refresh token).
            ticket.SetScopes(
                OpenIdConnectConstants.Scopes.Profile
//                ,
//                OpenIdConnectConstants.Scopes.OfflineAccess
                );

            context.Validate(ticket);
        }
    }
}