Owin.SiteMapping
===================

OWIN Middleware that provides functionality similar to IIS Site Bindings allowing you to partion your OWIN application by host header and port number.

Install via [nuget].

Using
-

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

Licence : [MIT]

Pull requests gratefully accepted.

Questions or suggestions? Create an issue or [@randompunter] on twitter.

  [nuget]: http://www.nuget.org/packages/Owin.SiteMapping/
  [MIT]: http://opensource.org/licenses/MIT
  [@randompunter]: http://twitter.com/randompunter
