namespace Owin.SiteMapping
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Owin.Testing;
    using Xunit;

    public class SiteMapTests
    {
        private readonly OwinTestServer _testServer;

        public SiteMapTests()
        {
            _testServer = OwinTestServer.Create(
                builder => builder.UseSiteMap(new SiteMap("localhost"),
                branch => branch.Use(context =>
                {
                    context.Response.StatusCode = 200;
                    context.Response.ReasonPhrase = "OK";
                    return Task.FromResult(0);
                })));
        }

        [Fact]
        public async Task Blah()
        {
            HttpClient httpClient = _testServer.CreateHttpClient();
            HttpResponseMessage response = await httpClient.GetAsync("http://localhost");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}