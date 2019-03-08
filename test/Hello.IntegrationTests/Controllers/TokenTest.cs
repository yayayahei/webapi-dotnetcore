using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Hello.DTOs;
using Hello.Models;
using Hello.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Hello.IntegrationTests.Controllers
{
    public class TokenTest : ApiTestBase
    {
        public TokenTest(TestWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async void Post_should_return_unauthorized_when_input_clientId_clientSecret_is_error()
        {
            using (var response = await GetToken("username", "password", "error_client_id", "error_client_secret"))
            {
                using (new AssertionScope())
                {
                    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                    var actualJson = response.Content.ReadAsStringAsync().Result;
                    var actual = JToken.Parse(actualJson);
                    var expected = JToken.FromObject(new
                    {
                        error = "invalid_request",
                    });
                    actual.Should().BeEquivalentTo(expected);
                }
            }
        }

        [Fact]
        public async void
            Post_should_return_error_when_input_username_password_is_error_clientId_clientSecret_is_valid()
        {
            var client = Factory.CreateClient();
            using (var response = await client.PostAsync("Token",
                new TokenRequest("error", "error", "client_id", "client_secret").ToFormUrlContent()))
            {
                using (new AssertionScope())
                {
                    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                    var actualJson = response.Content.ReadAsStringAsync().Result;
                    var actual = JToken.Parse(actualJson);
                    var expected = JToken.FromObject(new
                    {
                        error = "access_denied",
                        error_description = "Invalid user credentials."
                    });
                    actual.Should().BeEquivalentTo(expected);
                }
            }
        }

        [Fact]
        public async void
            Post_should_return_token_when_input_username_password_clientId_clientSecret_is_Admin_123456_client_id__client_secret()
        {
            var client = Factory.CreateClient();
            using (var response = await client.PostAsync("Token",
                new TokenRequest("Test", "123456", "client_id", "client_secret").ToFormUrlContent()))
            {
                using (new AssertionScope())
                {
                    response.StatusCode.Should().Be(HttpStatusCode.OK);
                    var actualJson = response.Content.ReadAsStringAsync().Result;
                    var actual = JToken.Parse(actualJson);
                    var expected = JToken.FromObject(new TokenResponse()
                    {
                        access_token = "sd",
                        expires_in = 24 * 60 * 60,
                        scope = "profile",
                        token_type = "Bearer"
                    });
                    actual.Should().BeEquivalentTo(expected);
                }
            }
        }


        private async Task<HttpResponseMessage> GetToken(string username, string password, string clientId,
            string clientSecret)
        {
            var client = Factory.CreateClient();
            return await client.PostAsync("Token",
                new TokenRequest(username, password, clientId, clientSecret).ToFormUrlContent());
        }
    }
}