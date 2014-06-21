namespace SiteMappingMiddleware
{
    using System;

    [Flags]
    public enum RequestScheme
    {
        Http = 1,
        Https = 2,
        HttpsXForwardedProto = 4 + 2
    }
}