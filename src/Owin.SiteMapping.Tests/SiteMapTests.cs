namespace Owin.SiteMapping
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Owin.Infrastructure;
    using Microsoft.Owin.Testing;
    using Xunit;

    public class SiteMapTests
    {
        private readonly TestServer _testServer;

        public SiteMapTests()
        {
            _testServer = TestServer.Create(
                builder =>
                {
                    SignatureConversions.AddConversions(builder); // supports Microsoft.Owin.OwinMiddleWare
                    builder.MapSite(new SiteMapConfig("example.com:81"),
                        branch => branch.Use((context, _) =>
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
            HttpClient httpClient = _testServer.HttpClient;
            HttpResponseMessage response = await httpClient.GetAsync("http://example.com:81");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_site_is_unknown_then_should_get_NotFound()
        {
            HttpClient httpClient = _testServer.HttpClient;
            HttpResponseMessage response = await httpClient.GetAsync("http://example.com:82");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}