namespace DH.Owin.SiteMapping.Tests
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Owin.Testing;
    using Xunit;
    using global::Owin;

    public class SiteMapTests
    {
        private readonly TestServer _testServer;

        public SiteMapTests()
        {
            _testServer = TestServer.Create(builder => builder.UseSiteMap(new SiteMap("localhost"), branch => branch.UseHandler((request, response) =>
                {
                    response.StatusCode = 200;
                    response.ReasonPhrase = "OK";
                })));
        }

        [Fact]
        public async Task Blah()
        {
            HttpResponseMessage response = await _testServer.HttpClient.GetAsync("http://localhost");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}