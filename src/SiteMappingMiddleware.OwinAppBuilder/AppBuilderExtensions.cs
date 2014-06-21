﻿namespace Owin
{
    using System;
    using System.Collections.Generic;
    using SiteMappingMiddleware;
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
    /// Represents a set of extension methods around <see cref="IAppBuilder"/> that expose limits middleware.
    /// </summary>
    public static class AppBuilderExtensions
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
        public static IAppBuilder MapSite(this IAppBuilder builder, string hostname, AppFunc branch)
        {
            builder.MustNotBeNull("builder");
            hostname.MustNotBeNullOrWhitespace("hostname");
            branch.MustNotBeNull("branch");

            builder
                .UseOwin()
                .MapSite(new[] {new SiteMapConfig(hostname)}, branch);
            return builder;
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
        public static IAppBuilder MapSite(this IAppBuilder builder, string hostname, Action<IAppBuilder> branch)
        {
            builder.MustNotBeNull("builder");
            hostname.MustNotBeNullOrWhitespace("hostname");
            branch.MustNotBeNull("branch");

            return MapSite(builder, new[] { new SiteMapConfig(hostname) }, builder.BranchConfig(branch));
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
        public static IAppBuilder MapSite(this IAppBuilder builder, string hostname, RequestScheme requestScheme, AppFunc branch)
        {
            builder.MustNotBeNull("builder");
            hostname.MustNotBeNullOrWhitespace("hostname");
            branch.MustNotBeNull("branch");

            builder
                .UseOwin()
                .MapSite(hostname, requestScheme, branch);
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
        public static IAppBuilder MapSite(this IAppBuilder builder, string hostname, RequestScheme requestScheme, Action<IAppBuilder> branch)
        {
            builder.MustNotBeNull("builder");
            hostname.MustNotBeNullOrWhitespace("hostname");
            branch.MustNotBeNull("branch");

            return MapSite(builder, hostname, requestScheme, builder.BranchConfig(branch));
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
        public static IAppBuilder MapSite(this IAppBuilder builder, SiteMapConfig siteMapConfig, AppFunc branch)
        {
            builder.MustNotBeNull("builder");
            siteMapConfig.MustNotBeNull("siteMapConfig");
            branch.MustNotBeNull("branch");

            builder
               .UseOwin()
               .MapSite(siteMapConfig, branch);
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
        public static IAppBuilder MapSite(this IAppBuilder builder, SiteMapConfig siteMapConfig, Action<IAppBuilder> branch)
        {
            builder.MustNotBeNull("builder");
            siteMapConfig.MustNotBeNull("siteMapConfig");
            branch.MustNotBeNull("branch");

            return MapSite(builder, siteMapConfig, builder.BranchConfig(branch));
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
        public static IAppBuilder MapSite(this IAppBuilder builder, IEnumerable<SiteMapConfig> siteMapConfigs, AppFunc branch)
        {
            builder.MustNotBeNull("builder");
            siteMapConfigs.MustNotBeNull("siteMapConfigs");
            branch.MustNotBeNull("branch");

            builder
               .UseOwin()
               .MapSite(siteMapConfigs, branch);
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
        public static IAppBuilder MapSite(this IAppBuilder builder, IEnumerable<SiteMapConfig> siteMapConfigs, Action<IAppBuilder> branch)
        {
            builder.MustNotBeNull("builder");
            siteMapConfigs.MustNotBeNull("siteMapConfigs");
            branch.MustNotBeNull("branch");

            return MapSite(builder, siteMapConfigs, builder.BranchConfig(branch));
        }

        private static BuildFunc UseOwin(this IAppBuilder builder)
        {
            return middleware => builder.Use(middleware(builder.Properties));
        }

        private static AppFunc BranchConfig(this IAppBuilder builder, Action<IAppBuilder> branchConfig)
        {
            var branchBuilder = builder.New();
            branchConfig(branchBuilder);
            return (AppFunc)branchBuilder.Build(typeof(AppFunc));
        }
    }
}