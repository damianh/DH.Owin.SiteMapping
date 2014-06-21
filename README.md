Site Mapping Middleware
===================

[![Build status](https://ci.appveyor.com/api/projects/status/mh0te6mpmwyvc0fn)](https://ci.appveyor.com/project/damianh/sitemappingmiddleware) [![NuGet Status](http://img.shields.io/nuget/v/SiteMappingMiddleware.svg?style=flat)](https://www.nuget.org/packages/SiteMappingMiddleware/) [![NuGet Status](http://img.shields.io/nuget/v/SiteMappingMiddleware.OwinAppBuilder.svg?style=flat)](https://www.nuget.org/packages/SiteMappingMiddleware.OwinAppBuilder/)

OWIN Middleware that provides functionality similar to IIS Site Bindings allowing you to partition your OWIN application by host header and port number.

#### Installation

There are two nuget packages. The main one is pure owin and this has no dependencies.

`install-package SiteMappingMiddleware`

The second package provides integration with IAppBuilder, which is deprecated but provided here for legacy and compatability reasons.

`install-package SiteMappingMiddleware.OwinAppBuilder`

An asp.net vNext builder integration package will be forthcoming.

#### Using

```csharp
public class Startup
{
  public void Configuration(IAppBuilder builder)
  {
    builder
      .MapSite("www.example.com", branch => branch.Use(...))
      .MapSite("admin.example.com", branch => branch.Use(...));
  }
}
```

#### Help

[@randompunter](https://twitter.com/randompunter) or [OWIN room on Jabbr](https://jabbr.net/#/rooms/owin)

##### Developed with:

[![Resharper](http://neventstore.org/images/logo_resharper_small.gif)](http://www.jetbrains.com/resharper/)
[![TeamCity](http://neventstore.org/images/logo_teamcity_small.gif)](http://www.jetbrains.com/teamcity/)
[![dotCover](http://neventstore.org/images/logo_dotcover_small.gif)](http://www.jetbrains.com/dotcover/)
[![dotTrace](http://neventstore.org/images/logo_dottrace_small.gif)](http://www.jetbrains.com/dottrace/)