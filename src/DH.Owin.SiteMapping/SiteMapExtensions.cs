// ReSharper disable CheckNamespace
namespace Owin
// ReSharper restore CheckNamespace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DH.Owin.SiteMapping;
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public static class SiteMapExtensions
    {
        public static IAppBuilder UseSiteMap<TApp>(this IAppBuilder builder, SiteMap siteMap, TApp branchApp)
            where TApp : class
        {
            return UseSiteMap(builder, new[] {siteMap}, branchApp);
        }

        public static IAppBuilder UseSiteMap<TApp>(this IAppBuilder builder, IEnumerable<SiteMap> siteMaps, TApp branchApp)
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
            return builder.UseType<SiteMapMiddleware>(branchBuilder.Build(typeof(AppFunc)), siteMaps);
        }

        public static IAppBuilder UseSiteMap(this IAppBuilder builder, SiteMap siteMap, Action<IAppBuilder> branchConfig)
        {
            return UseSiteMap(builder, new[] { siteMap }, branchConfig);
        }

        public static IAppBuilder UseSiteMap(this IAppBuilder builder, IEnumerable<SiteMap> siteMaps, Action<IAppBuilder> branchConfig)
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
            return builder.UseType<SiteMapMiddleware>(branchBuilder.Build(typeof(AppFunc)), siteMaps);
        }
    }
}