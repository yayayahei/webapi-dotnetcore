using System;
using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Primitives;
using Hello.Events;
using Hello.Providers;
using Hello.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hello
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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
            
            services.AddHello(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}