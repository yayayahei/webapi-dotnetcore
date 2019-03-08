using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Hello.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Hello.IntegrationTests.Controllers
{
    public class ApiTestBase : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        internal readonly TestWebApplicationFactory<Startup> Factory;

        internal ApiTestBase(TestWebApplicationFactory<Startup> factory)
        {
            Factory = factory;
        }

        internal WebApplicationFactory<Startup> RegisterMockComponents(Action<IServiceCollection> serviceCollection)
        {
            return Factory.WithWebHostBuilder(builder => { builder.ConfigureTestServices(serviceCollection); });
        }

        internal TokenResponse Authorize(HttpClient client, string username, string password, string clientId,
            string clientSecret)
        {
            var token = GetToken(username, password, clientId, clientSecret);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(token.token_type, token.access_token);
            return token;
        }

        internal TokenResponse GetToken(string username, string password, string clientId, string clientSecret)
        {
            var client = Factory.CreateClient();
            using (var response = client.PostAsync("Token",
                    new TokenRequest(username, password, clientId, clientSecret).ToFormUrlContent())
                .Result)
            {
                var responseContent = response.Content?.ReadAsStringAsync().Result;
                var responseJToken = responseContent != "" ? JToken.Parse(responseContent) : null;
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"{response.StatusCode},未能获取数据", null);
                var result = responseJToken?.ToObject<TokenResponse>();
                return result;
            }
        }
    }
}