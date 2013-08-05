namespace Owin.SiteMapping
{
    using System;

    public class SiteMapConfig : IEquatable<SiteMapConfig>
    {
        private readonly string _hostName;
        private readonly int _port;
        private readonly RequestScheme _requestScheme;

        public SiteMapConfig(string hostName, RequestScheme requestScheme = RequestScheme.Http, int port = 80)
        {
            if (string.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentException("hostName must not be null or whitespace");
            }
            _requestScheme = requestScheme;
            _port = port;
            _hostName = hostName;
        }

        public int Port
        {
            get { return _port; }
        }

        public string HostName
        {
            get { return _hostName; }
        }

        public RequestScheme RequestScheme
        {
            get { return _requestScheme; }
        }

        public bool Equals(SiteMapConfig other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_hostName, other._hostName) && _port == other._port && _requestScheme == other._requestScheme;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SiteMapConfig) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (_hostName != null ? _hostName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ _port;
                hashCode = (hashCode*397) ^ (int) _requestScheme;
                return hashCode;
            }
        }

        public static bool operator ==(SiteMapConfig left, SiteMapConfig right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SiteMapConfig left, SiteMapConfig right)
        {
            return !Equals(left, right);
        }
    }
}