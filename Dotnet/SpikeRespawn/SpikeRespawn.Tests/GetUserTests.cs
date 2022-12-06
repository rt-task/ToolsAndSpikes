using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SpikeRespawn.Tests;
using Xunit;

namespace SpikeRespawn.Tests
{
    public class GetUserTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly string _url;

        public GetUserTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _url = "https://localhost:5001/Users/{id}";
            TestHelpers.ResetDatabaseWithRespawn(factory,
                "Server=(localdb)\\mssqllocaldb;Database=RespawnSpike;Trusted_Connection=True;").Wait();

            //TestHelpers.ResetDatabase(factory).Wait();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task Should_Return_200OK_User(int userId)
        {
            var response = await _client.GetAsync(
                _url.Replace("{id}", userId.ToString()));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}