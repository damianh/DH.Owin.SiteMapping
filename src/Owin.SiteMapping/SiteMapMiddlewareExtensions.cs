namespace Owin.SiteMapping
{
    using System;
    using System.Collections.Generic;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
    using MidFunc = System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>;
    using BuildFunc = System.Action<System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>>;

    /// <summary>
    /// Set of helper extensions
    /// </summary>
    public static class SiteMapMiddlewareExtensions
    {
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

            builder(SiteMapMiddleware.MapSite(new[] { new SiteMapConfig(hostname) }, branch));
            return builder;
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

            builder(SiteMapMiddleware.MapSite(new[] { new SiteMapConfig(hostname, requestScheme) }, branch));
            return builder;
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

            builder(SiteMapMiddleware.MapSite(new[] { siteMapConfig }, branch));
            return builder;
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

            builder(SiteMapMiddleware.MapSite(siteMapConfigs, branch));
            return builder;
        }
    }
}