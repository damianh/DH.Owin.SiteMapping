using System;

namespace Owin.SiteMapping
{
    [Flags]
    public enum RequestScheme
    {
        Http = 1,
        Https = 2,
        HttpsXForwardedProto = 4 + 2
    }
}