namespace Owin.SiteMapping
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Owin.Infrastructure;
    using Testing;
    using Xunit;

    public class SiteMapTests
    {
        private readonly OwinTestServer _testServer;

        public SiteMapTests()
        {
            _testServer = OwinTestServer.Create(
                builder =>
                {
                    SignatureConversions.AddConversions(builder); // supports Microsoft.Owin.OwinMiddleWare
                    builder.MapSite(new SiteMapConfig("example.com"),
                        branch => branch.Use(context =>
                        {
                            context.Response.StatusCode = 200;
                            context.Response.ReasonPhrase = "OK";
                            return Task.FromResult(0);
                        }));
                });
        }

        [Fact]
        public async Task When_site_is_mapped_then_should_get_OK()
        {
            HttpClient httpClient = _testServer.CreateHttpClient();
            HttpResponseMessage response = await httpClient.GetAsync("http://example.com");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_site_is_unknown_then_should_get_NotFound()
        {
            HttpClient httpClient = _testServer.CreateHttpClient();
            HttpResponseMessage response = await httpClient.GetAsync("http://example.com");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}