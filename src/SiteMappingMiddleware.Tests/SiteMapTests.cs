namespace SiteMappingMiddleware
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Testing;
    using Xunit;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public class SiteMapTests
    {
        private static AppFunc UseOwinContext(Func<IOwinContext, Task> action)
        {
            return env =>
            {
                var context = new OwinContext(env);
                return action(context);
            };
        }

        [Fact]
        public async Task When_site_is_mapped_then_should_get_OK()
        {
            var testServer = TestServer.Create(
                builder => builder.MapSite("example.com:81", UseOwinContext(context =>
                {
                    context.Response.StatusCode = 200;
                    context.Response.ReasonPhrase = "OK";
                    return Task.FromResult(0);
                })));

            HttpClient httpClient = testServer.HttpClient;

            HttpResponseMessage response = await httpClient.GetAsync("http://example.com:81");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_site_is_unknown_then_should_get_NotFound()
        {
            var testServer = TestServer.Create(
                builder => builder.MapSite("example.com:81", UseOwinContext(context =>
                {
                    context.Response.StatusCode = 200;
                    context.Response.ReasonPhrase = "OK";
                    return Task.FromResult(0);
                })));
            HttpClient httpClient = testServer.HttpClient;

            HttpResponseMessage response = await httpClient.GetAsync("http://example.com:82");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task When_secure_site_is_mapped_on_https_then_should_get_OK()
        {
            var testServer = TestServer.Create(
                builder => builder.MapSite("secure.example.com", RequestScheme.Https,
                    branch => branch.Use((context, _) =>
                    {
                        context.Response.StatusCode = 200;
                        context.Response.ReasonPhrase = "OK";
                        return Task.FromResult(0);
                    })));
            HttpClient httpClient = testServer.HttpClient;

            HttpResponseMessage response = await httpClient.GetAsync("https://secure.example.com");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_secure_site_is_mapped_on_http_then_should_get_forbidden()
        {
            var testServer = TestServer.Create(
                builder => builder.MapSite("secure.example.com",
                    branch => branch.Use((context, _) =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ReasonPhrase = "Forbidden";
                        return Task.FromResult(0);
                    })));
            HttpClient httpClient = testServer.HttpClient;

            HttpResponseMessage response = await httpClient.GetAsync("http://secure.example.com");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task When_secure_site_is_mapped_on_https_with_x_forward_proto_header_then_should_get_Ok()
        {
            var testServer = TestServer.Create(
                builder => builder.MapSite("secure.example.com", RequestScheme.HttpsXForwardedProto,
                    branch => branch.Use((context, _) =>
                    {
                        context.Response.StatusCode = 200;
                        context.Response.ReasonPhrase = "OK";
                        return Task.FromResult(0);
                    })));
            HttpClient httpClient = testServer.HttpClient;
            httpClient.DefaultRequestHeaders.Add("X-Forwarded-Proto", "https");

            HttpResponseMessage response = await httpClient.GetAsync("http://secure.example.com");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_secure_site_is_mapped_on_http_with_x_forward_proto_header_then_should_get_forbidden()
        {
            var testServer = TestServer.Create(
                builder => builder.MapSite("secure.example.com",
                    branch => branch.Use((context, _) =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ReasonPhrase = "Forbidden";
                        return Task.FromResult(0);
                    })));
            HttpClient httpClient = testServer.HttpClient;
            httpClient.DefaultRequestHeaders.Add("X-Forwarded-Proto", "http");

            HttpResponseMessage response = await httpClient.GetAsync("http://secure.example.com");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task When_multisecure_site_is_mapped_on_https_then_should_get_OK()
        {
            var testServer = TestServer.Create(
                builder => builder.MapSite("multisecure.example.com", RequestScheme.HttpsXForwardedProto | RequestScheme.Https,
                    branch => branch.Use((context, _) =>
                    {
                        context.Response.StatusCode = 200;
                        context.Response.ReasonPhrase = "OK";
                        return Task.FromResult(0);
                    })));
            HttpClient httpClient = testServer.HttpClient;

            HttpResponseMessage response = await httpClient.GetAsync("https://multisecure.example.com");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_multisecure_site_is_mapped_on_http_then_should_get_forbidden()
        {
            var testServer = TestServer.Create(
                builder => builder.MapSite("multisecure.example.com",
                    branch => branch.Use((context, _) =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ReasonPhrase = "Forbidden";
                        return Task.FromResult(0);
                    })));
            HttpClient httpClient = testServer.HttpClient;

            HttpResponseMessage response = await httpClient.GetAsync("http://multisecure.example.com");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task When_multisecure_site_is_mapped_on_https_with_x_forward_proto_header_then_should_get_Ok()
        {
            var testServer = TestServer.Create(
                builder => builder.MapSite("multisecure.example.com", RequestScheme.HttpsXForwardedProto | RequestScheme.Https,
                   branch => branch.Use((context, _) =>
                    {
                        context.Response.StatusCode = 200;
                        context.Response.ReasonPhrase = "OK";
                        return Task.FromResult(0);
                    })));
            HttpClient httpClient = testServer.HttpClient;
            httpClient.DefaultRequestHeaders.Add("X-Forwarded-Proto", "https");

            HttpResponseMessage response = await httpClient.GetAsync("http://multisecure.example.com");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task When_multisecure_site_is_mapped_on_http_with_x_forward_proto_header_then_should_get_forbidden()
        {
            var testServer = TestServer.Create(
                builder => builder.MapSite("multisecure.example.com",
                    branch => branch.Use((context, _) =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ReasonPhrase = "Forbidden";
                        return Task.FromResult(0);
                    })));
            HttpClient httpClient = testServer.HttpClient;
            httpClient.DefaultRequestHeaders.Add("X-Forwarded-Proto", "http");

            HttpResponseMessage response = await httpClient.GetAsync("http://multisecure.example.com");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}