namespace Owin.SiteMapping
{
    using System;

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