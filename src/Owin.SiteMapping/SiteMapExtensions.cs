// ReSharper disable CheckNamespace
namespace Owin
// ReSharper restore CheckNamespace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SiteMapping;
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public static class SiteMapExtensions
    {
        public static IAppBuilder MapSite<TApp>(this IAppBuilder builder, SiteMapConfig siteMapConfig, TApp branchApp)
            where TApp : class
        {
            return MapSite(builder, new[] {siteMapConfig}, branchApp);
        }

        public static IAppBuilder MapSite<TApp>(this IAppBuilder builder, IEnumerable<SiteMapConfig> siteMaps, TApp branchApp)
            where TApp : class
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (siteMaps == null)
            {
                throw new ArgumentNullException("siteMaps");
            }
            if (!siteMaps.Any())
            {
                throw new ArgumentException("siteMaps must not be empty");
            }
            if (branchApp == null)
            {
                throw new ArgumentNullException("branchApp");
            }

            var branchBuilder = builder.New();
            branchBuilder.Use(new Func<TApp, TApp>(_ => branchApp));
            var appFunc = (AppFunc)branchBuilder.Build(typeof(AppFunc));
            return builder.Use<SiteMapMiddleware>(appFunc, siteMaps);
        }

        public static IAppBuilder MapSite(this IAppBuilder builder, string hostName, Action<IAppBuilder> branchConfig)
        {
            return MapSite(builder, new SiteMapConfig(hostName), branchConfig);
        }

        public static IAppBuilder MapSite(this IAppBuilder builder, string hostName, RequestScheme requestScheme, Action<IAppBuilder> branchConfig)
        {
            return MapSite(builder, new SiteMapConfig(hostName, requestScheme), branchConfig);
        }

        public static IAppBuilder MapSite(this IAppBuilder builder, SiteMapConfig siteMapConfig, Action<IAppBuilder> branchConfig)
        {
            return MapSite(builder, new[] { siteMapConfig }, branchConfig);
        }

        public static IAppBuilder MapSite(this IAppBuilder builder, IEnumerable<SiteMapConfig> siteMaps, Action<IAppBuilder> branchConfig)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (siteMaps == null)
            {
                throw new ArgumentNullException("siteMaps");
            }
            if (!siteMaps.Any())
            {
                throw new ArgumentException("siteMaps must not be empty");
            }
            if (branchConfig == null)
            {
                throw new ArgumentNullException("branchConfig");
            }

            var branchBuilder = builder.New();
            branchConfig(branchBuilder);
            var appFunc = (AppFunc)branchBuilder.Build(typeof(AppFunc));
            return builder.Use<SiteMapMiddleware>(appFunc, siteMaps);
        }
    }
}