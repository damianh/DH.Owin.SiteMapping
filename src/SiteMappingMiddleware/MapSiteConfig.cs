namespace SiteMappingMiddleware
{
    using System;

    public class MapSiteConfig : IEquatable<MapSiteConfig>
    {
        private readonly string _hostName;
        private readonly RequestScheme _requestScheme;

        /// <summary>
        ///     Repressents a site map.
        /// </summary>
        /// <param name="hostName">
        ///     The hostname this site map is associated with. Include a port number if non standard port is
        ///     used. For example "example.com:81"
        /// </param>
        /// <param name="requestScheme">The request scheme this site responds too.</param>
        /// <exception cref="ArgumentException"></exception>
        public MapSiteConfig(string hostName, RequestScheme requestScheme = RequestScheme.Http)
        {
            if (string.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentException("hostName must not be null or whitespace");
            }
            if (!hostName.Contains(":"))
            {
                hostName = hostName + (requestScheme.HasFlag(RequestScheme.Https) ? ":443" : ":80");
            }
            _requestScheme = requestScheme;
            _hostName = hostName;
        }

        public bool Equals(MapSiteConfig other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_hostName, other._hostName) && _requestScheme.HasFlag(other._requestScheme);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((MapSiteConfig)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (_hostName != null ? _hostName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int)_requestScheme & 3; //filter http & https only as X-Forwarded-Proto logically ors to https 
                return hashCode;
            }
        }

        public static bool operator ==(MapSiteConfig left, MapSiteConfig right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MapSiteConfig left, MapSiteConfig right)
        {
            return !Equals(left, right);
        }
    }
}