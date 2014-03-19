﻿namespace Owin.SiteMapping
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Microsoft.Owin;

	public class SiteMapMiddleware
	{
		private readonly Func<IDictionary<string, object>, Task> _next;
		private readonly Func<IDictionary<string, object>, Task> _branch;
		private readonly HashSet<SiteMapConfig> _siteMaps;

		public SiteMapMiddleware(
			Func<IDictionary<string, object>, Task> next,
			Func<IDictionary<string, object>, Task> branch,
			IEnumerable<SiteMapConfig> siteMaps)
		{
			if (next == null)
			{
				throw new ArgumentNullException("next");
			}
			if (branch == null)
			{
				throw new ArgumentNullException("branch");
			}
			if (siteMaps == null)
			{
				throw new ArgumentNullException("siteMaps");
			}

			_next = next;
			_branch = branch;
			_siteMaps = new HashSet<SiteMapConfig>(siteMaps);
		}

		public Task Invoke(IDictionary<string, object> environment)
		{
			if (environment == null)
			{
				throw new ArgumentNullException("environment");
			}
			var request = new OwinRequest(environment);

			//If the headers have a X-Forwarded-Proto header then the request has been mapped through a load balancer and the initial scheme is conatined in the header value.
			var scheme = string.Equals(request.Headers["X-Forwarded-Proto"], "https", StringComparison.InvariantCultureIgnoreCase) ? "HttpsXForwardedProto" : request.Scheme;

			var requestScheme = (RequestScheme)Enum.Parse(typeof(RequestScheme), scheme, true);
			var siteMap = new SiteMapConfig(request.Host.Value, requestScheme);
			return _siteMaps.Contains(siteMap) ? _branch(request.Environment) : _next(request.Environment);
		}
	}
}