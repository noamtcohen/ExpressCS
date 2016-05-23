using System.Collections.Generic;
using Microsoft.Owin;

namespace ExpressCS
{
    public interface IMiddleware
    {
        bool Match(IOwinContext context,Router parent);
        IEnumerable<RouteHandler> GetStack(IOwinContext context);
    }
}