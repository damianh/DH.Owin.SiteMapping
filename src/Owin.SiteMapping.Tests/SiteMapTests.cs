namespace Owin.SiteMapping
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
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
                    builder.MapSite(new SiteMapConfig("example.com:81"),
                        branch => branch.Use((context, _) =>
                                             {
                                                 context.Response.StatusCode = 200;
                                                 context.Response.ReasonPhrase = "OK";
                                                 return Task.FromResult(0);
                                             }));

                    builder.MapSite(new SiteMapConfig("secure.example.com", RequestScheme.Https),
                        branch => branch.Use((context, _) =>
                                             {
                                                 context.Response.StatusCode = 200;
                                                 context.Response.ReasonPhrase = "OK";
                                                 return Task.FromResult(0);
                                             }));

                    builder.MapSite(new SiteMapConfig("secure.example.com", RequestScheme.HttpsXForwardedProto),
                        branch => branch.Use((context, _) =>
                                             {
                                                 context.Response.StatusCode = 200;
                                                 context.Response.ReasonPhrase = "OK";
                                                 return Task.FromResult(0);
                                             }));

                    builder.MapSite(new SiteMapConfig("secure.example.com"),
                        branch => branch.Use((context, _) =>
                                             {
                                                 context.Response.StatusCode = 403;
                                                 context.Response.ReasonPhrase = "Forbidden";
                                                 return Task.FromResult(0);
                                             }));

                    builder.MapSite(new SiteMapConfig("multisecure.example.com", RequestScheme.HttpsXForwardedProto | RequestScheme.Https),
                        branch => branch.Use((context, _) =>
                                             {
                                                 context.Response.StatusCode = 200;
                                                 context.Response.ReasonPhrase = "OK";
                                                 return Task.FromResult(0);
                                             }));

                    builder.MapSite(new SiteMapConfig("multisecure.example.com"),
                        branch => branch.Use((context, _) =>
                                             {
                                                 context.Response.StatusCode = 403;
                                                 context.Response.ReasonPhrase = "Forbidden";
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

        [Fact]
        public async Task When_secure_site_is_mapped_on_https_then_should_get_OK()
        {
            HttpClient httpClient = _testServer.HttpClient;
            HttpResponseMessage response = await httpClient.GetAsync("https://secure.example.com");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_secure_site_is_mapped_on_http_then_should_get_forbidden()
        {
            HttpClient httpClient = _testServer.HttpClient;
            HttpResponseMessage response = await httpClient.GetAsync("http://secure.example.com");
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task When_secure_site_is_mapped_on_https_with_x_forward_proto_header_then_should_get_Ok()
        {
            HttpClient httpClient = _testServer.HttpClient;
            httpClient.DefaultRequestHeaders.Add("X-Forwarded-Proto", "https");
            HttpResponseMessage response = await httpClient.GetAsync("http://secure.example.com");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_secure_site_is_mapped_on_http_with_x_forward_proto_header_then_should_get_forbidden()
        {
            HttpClient httpClient = _testServer.HttpClient;
            httpClient.DefaultRequestHeaders.Add("X-Forwarded-Proto", "http");
            HttpResponseMessage response = await httpClient.GetAsync("http://secure.example.com");
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task When_multisecure_site_is_mapped_on_https_then_should_get_OK()
        {
            HttpClient httpClient = _testServer.HttpClient;
            HttpResponseMessage response = await httpClient.GetAsync("https://multisecure.example.com");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_multisecure_site_is_mapped_on_http_then_should_get_forbidden()
        {
            HttpClient httpClient = _testServer.HttpClient;
            HttpResponseMessage response = await httpClient.GetAsync("http://multisecure.example.com");
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task When_multisecure_site_is_mapped_on_https_with_x_forward_proto_header_then_should_get_Ok()
        {
            HttpClient httpClient = _testServer.HttpClient;
            httpClient.DefaultRequestHeaders.Add("X-Forwarded-Proto", "https");
            HttpResponseMessage response = await httpClient.GetAsync("http://multisecure.example.com");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_multisecure_site_is_mapped_on_http_with_x_forward_proto_header_then_should_get_forbidden()
        {
            HttpClient httpClient = _testServer.HttpClient;
            httpClient.DefaultRequestHeaders.Add("X-Forwarded-Proto", "http");
            HttpResponseMessage response = await httpClient.GetAsync("http://multisecure.example.com");
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}