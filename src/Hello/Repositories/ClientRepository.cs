using System.Collections.Generic;
using System.Linq;
using Hello.Models;
using Microsoft.Extensions.Configuration;

namespace Hello.Repositories
{
    public class ClientRepository
    {
        private readonly IConfiguration _configuration;

        public ClientRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Client GetClient(string clientId, string clientSecret)
        {
            List<Client> clients = new List<Client>();
            _configuration.GetSection("Clients").Bind(clients);

            var result = clients.FirstOrDefault(client =>
                clientId.Equals(client.client_id) &&
                clientSecret.Equals(client.client_secret));
            
            return result;
        }
    }
}