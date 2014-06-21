﻿namespace Owin.SiteMapping
{
    using System;
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
    using MidFunc = System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>;
    using BuildFunc = System.Action<System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>>;

    internal static class AppBuilderExtensions
    {
        internal static BuildFunc Use(this IAppBuilder builder)
        {
            return middleware => builder.Use(middleware);
        }

        internal static IAppBuilder Use(this Action<MidFunc> middleware, IAppBuilder builder)
        {
            return builder;
        }

        internal static AppFunc BranchConfig(this IAppBuilder builder, Action<IAppBuilder> branchConfig)
        {
            var branchBuilder = builder.New();
            branchConfig(branchBuilder);
            return (AppFunc)branchBuilder.Build(typeof(AppFunc));
        }
    }
}