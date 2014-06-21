namespace SiteMappingMiddleware
{
    using System;
    using System.Collections.Generic;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
    using MidFunc = System.Func<
        System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>,
        System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>
        >;
    using BuildFunc = System.Action<
        System.Func<
            System.Collections.Generic.IDictionary<string, object>,
            System.Func<
                System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>,
                System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>
        >>>;

    /// <summary>
    /// 
    /// </summary>
    public static class SiteMapMiddleware
    {
        private const string OwinRequestHeadersKey = "owin.RequestHeaders";
        private const string OwinRequestSchemeKey = "owin.RequestScheme";
        private const string XForwardedProtoHeaderKey = "X-Forwarded-Proto";
        private const string HostHeaderKey = "Host";

        /// <summary>
        /// Maps the site.
        /// </summary>
        /// <param name="siteMapConfigs">The site map configs.</param>
        /// <param name="branch">The branch that is invoked when a request matches a site map config.</param>
        /// <returns>A middleware func.</returns>
        /// <exception cref="System.ArgumentNullException">siteMapConfigs</exception>
        /// <exception cref="System.ArgumentNullException">branch</exception>
        public static MidFunc MapSite(IEnumerable<SiteMapConfig> siteMapConfigs, AppFunc branch)
        {
            siteMapConfigs.MustNotBeNull("siteMapConfigs");
            branch.MustNotBeNull("branch");

            var siteMapsHashSet = new HashSet<SiteMapConfig>(siteMapConfigs);
            return
                next =>
                env =>
                {
                    var headers = env.Get<IDictionary<string, string[]>>(OwinRequestHeadersKey);
                    var host = headers[HostHeaderKey][0];
                    var scheme = env.Get<string>(OwinRequestSchemeKey);
                    //If the headers have a X-Forwarded-Proto header then the request has been mapped through
                    //a load balancer or reverse proxy and the initial scheme is contained in the header value.
                    string[] headerValues;
                    if (headers.TryGetValue(XForwardedProtoHeaderKey, out headerValues))
                    {
                        string headerValue = headerValues[0];
                        if (string.Equals(headerValue, "https", StringComparison.InvariantCultureIgnoreCase))
                        {
                            scheme = "HttpsXForwardedProto";
                        }
                    }
                    var requestScheme = (RequestScheme)Enum.Parse(typeof(RequestScheme), scheme, true);
                    var siteMap = new SiteMapConfig(host, requestScheme);
                    return siteMapsHashSet.Contains(siteMap)
                        ? branch(env)
                        : next(env);
                };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="hostname"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        /// <exception cref="System.ArgumentException">hostname</exception>
        /// <exception cref="System.ArgumentNullException">branch</exception>
        public static BuildFunc MapSite(this BuildFunc builder, string hostname, AppFunc branch)
        {
            builder.MustNotBeNull("builder");
            hostname.MustNotBeNullOrWhitespace("hostname");
            branch.MustNotBeNull("branch");

            return MapSite(builder, new[] {new SiteMapConfig(hostname)}, branch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="hostname"></param>
        /// <param name="requestScheme"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        /// <exception cref="System.ArgumentException">hostname</exception>
        /// <exception cref="System.ArgumentNullException">branch</exception>
        public static BuildFunc MapSite(this BuildFunc builder, string hostname, RequestScheme requestScheme, AppFunc branch)
        {
            builder.MustNotBeNull("builder");
            hostname.MustNotBeNullOrWhitespace("hostname");
            branch.MustNotBeNull("branch");

            return MapSite(builder, new SiteMapConfig(hostname, requestScheme), branch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="siteMapConfig"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        /// <exception cref="System.ArgumentNullException">siteMapConfig</exception>
        /// <exception cref="System.ArgumentNullException">branch</exception>
        public static BuildFunc MapSite(this BuildFunc builder, SiteMapConfig siteMapConfig, AppFunc branch)
        {
            builder.MustNotBeNull("builder");
            siteMapConfig.MustNotBeNull("siteMapConfig");
            branch.MustNotBeNull("branch");

            return MapSite(builder, new[] {siteMapConfig}, branch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="siteMapConfigs"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        /// <exception cref="System.ArgumentNullException">siteMapConfigs</exception>
        /// <exception cref="System.ArgumentNullException">branch</exception>
        public static BuildFunc MapSite(this BuildFunc builder, IEnumerable<SiteMapConfig> siteMapConfigs, AppFunc branch)
        {
            builder.MustNotBeNull("builder");
            siteMapConfigs.MustNotBeNull("siteMapConfigs");
            branch.MustNotBeNull("branch");

            builder(_ => MapSite(siteMapConfigs, branch));
            return builder;
        }
    }
}