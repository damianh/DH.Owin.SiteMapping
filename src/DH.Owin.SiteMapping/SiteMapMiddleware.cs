namespace DH.Owin.SiteMapping
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Owin.Types;
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public class SiteMapMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> _nextApp;
        private readonly AppFunc _branch;
        private readonly HashSet<SiteMap> _siteMaps;

        public SiteMapMiddleware(AppFunc nextApp, AppFunc branch, IEnumerable<SiteMap> siteMaps)
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