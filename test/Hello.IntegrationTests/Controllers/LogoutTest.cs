using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Hello.IntegrationTests.Controllers
{
    public class LogoutTest : ApiTestBase
    {
        public LogoutTest(TestWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public void Logout_should_return_ok_when_has_been_called_with_token()
        {
            var client = Factory.CreateClient();
            var token = Authorize(client, "Test", "123456", "client_id", "client_secret");
            using (new AssertionScope())
            {
                using (var response = client.PostAsync("Logout", new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("client_id", "client_id"),
                    new KeyValuePair<string, string>("client_secret", "client_secret"),
                    new KeyValuePair<string, string>("token", token.access_token),
                })).Result)
                {
                    response.StatusCode.Should().Be(HttpStatusCode.OK);
                }

                using (var response = client.GetAsync("api/Authorized").Result)
                {
                    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
                }
            }
        }

        [Fact]
        public void Authorized_should_return_unauthorized_when_token_is_empty()
        {
            var client = Factory.CreateClient();
            using (var response = client.GetAsync("api/Authorized").Result)
            {
                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }
    }
}