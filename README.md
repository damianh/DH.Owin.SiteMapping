Owin.SiteMapping
===================

OWIN Middleware that provides functionality similar to IIS Site Bindings allowing you to partion your OWIN application by host header and port number.

Using
-

```csharp
public class Startup
{
  public void Configuration(IAppBuilder builder)
  {
    builder
      .UseSiteMap(new SiteMap("www.example.com"), branch => branch.Use(...))
      .UseSiteMap(new SiteMap("admin.example.com"), branch => branch.Use(...));
  }
}
```

Licence : [MIT]

Questions? [@randompunter]

  [MIT]: http://opensource.org/licenses/MIT
  [@randompunter]: http://twitter.com/randompunter
