namespace SiteMappingMiddleware
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Owin.Testing;
    using Owin;
    using Xunit;

    public class AppBuilderExtensionsTests
    {
        [Fact]
        public async Task Can_map_site_with_SiteMapConfig_and_IAppBuilder_branch()
        {
            var testServer = TestServer.Create(
                builder => builder.MapSite(new MapSiteConfig("example.com"),
                    branch => branch.Use((context, _) =>
                    {
                        context.Response.StatusCode = 200;
                        context.Response.ReasonPhrase = "OK";
                        return Task.FromResult(0);
                    })));
            HttpClient httpClient = testServer.HttpClient;

            HttpResponseMessage response = await httpClient.GetAsync("http://example.com");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Can_map_site_with_SiteMapConfigs_and_IAppBuilder_branch()
        {
            var testServer = TestServer.Create(
                builder => builder.MapSite(new [] { new MapSiteConfig("site1.example.com"), new MapSiteConfig("site2.example.com")},
                    branch => branch.Use((context, _) =>
                    {
                        context.Response.StatusCode = 200;
                        context.Response.ReasonPhrase = "OK";
                        return Task.FromResult(0);
                    })));
            HttpClient httpClient = testServer.HttpClient;

            HttpResponseMessage response = await httpClient.GetAsync("http://site1.example.com");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            response = await httpClient.GetAsync("http://site2.example.com");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            response = await httpClient.GetAsync("http://site3.example.com");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}