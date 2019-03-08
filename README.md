# webapi-dotnetcore

A demo project to provide restful api using dotnet core 2.1.

## Features

### Authorization
#### Prerequisites
* AspNet.Security.OAuth.Validation
* AspNet.Security.OpenIdConnect.Server

#### Steps
##### use middleware

```
public void Configure(IApplicationBuilder app, IHostingEnvironment env){
    ...
    app.UseAuthentication();
    app.UseMvc();
    ...
}
```

##### configure authentication

```
public void ConfigureServices(IServiceCollection services){
    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    services.AddAuthentication(OAuthValidationDefaults.AuthenticationScheme)
        .AddOAuthValidation(options => { options.EventsType = typeof(ValidationEvents); })
        .AddOpenIdConnectServer(options =>
        {
            // Enable the token endpoint.
            options.TokenEndpointPath = "/Token";
            options.RevocationEndpointPath = "/Logout";
            options.ProviderType = typeof(AuthorizationProvider);
            options.AllowInsecureHttp = true;
            options.AccessTokenLifetime = TimeSpan.FromDays(1);
        });

    services.AddScoped<AuthorizationProvider>();
    services.AddScoped<ValidationEvents>();
    services.AddScoped<UserRepository>();
    services.AddScoped<ClientRepository>(sp => new ClientRepository(Configuration));
    services.AddScoped<TokenRepository>();
}

```

##### Add AuthorizationProvider
The class is derived from OpenIdConnectServerProvider, is used to generate token, revoke token...

Methods invoked order:

```
ExtractTokenRequest
ValidateTokenRequest
HandleTokenRequest
SignInAsync
SerializeAccessTokenAsync
SendTokenResponseAsync
ApplyTokenResponse
SendPayloadAsync
```

##### Add ValidationEvents
The class is derived from OAuthValidationEvents, is used to decrypt token, validate token...

Use OnDecryptToken event to check the token is revoked or not, then you can make the decryption fail or not.

```
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
```

## Tests