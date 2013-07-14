namespace Owin.SiteMapping
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Owin;

    public class SiteMapMiddleware : OwinMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> _branch;
        private readonly HashSet<SiteMap> _siteMaps;

        public SiteMapMiddleware(OwinMiddleware next, Func<IDictionary<string, object>, Task> branch, IEnumerable<SiteMap> siteMaps)
            : base(next)
        {
            if (branch == null)
            {
                throw new ArgumentNullException("branch");
            }
            if (siteMaps == null)
            {
                throw new ArgumentNullException("siteMaps");
            }

            _branch = branch;
            _siteMaps = new HashSet<SiteMap>(siteMaps);
        }

        public override Task Invoke(IOwinContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var requestScheme = (RequestScheme)Enum.Parse(typeof(RequestScheme), context.Request.Scheme, true);
            var port = context.Request.LocalPort;
            var siteMap = new SiteMap(context.Request.Host, requestScheme, port ?? 80);
            return _siteMaps.Contains(siteMap) ? _branch(context.Environment) : Next.Invoke(context);
        }
    }
}