namespace Owin.SiteMapping
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Owin.Types;

    public class SiteMapMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> _nextApp;
        private readonly Func<IDictionary<string, object>, Task> _branch;
        private readonly HashSet<SiteMap> _siteMaps;

        public SiteMapMiddleware(Func<IDictionary<string, object>, Task> nextApp, Func<IDictionary<string, object>, Task> branch, IEnumerable<SiteMap> siteMaps)
        {
            if (nextApp == null)
            {
                throw new ArgumentNullException("nextApp");
            }
            if (branch == null)
            {
                throw new ArgumentNullException("branch");
            }
            if (siteMaps == null)
            {
                throw new ArgumentNullException("siteMaps");
            }

            _nextApp = nextApp;
            _branch = branch;
            _siteMaps = new HashSet<SiteMap>(siteMaps);
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            var owinRequest = new OwinRequest(environment);
            var requestScheme = (RequestScheme)Enum.Parse(typeof (RequestScheme), owinRequest.Scheme, true);
            var port = owinRequest.Get<int>(OwinConstants.CommonKeys.LocalPort);
            var siteMap = new SiteMap(owinRequest.Host, requestScheme, port);
            return _siteMaps.Contains(siteMap) ? _branch(environment) : _nextApp(environment);
        }
    }
}