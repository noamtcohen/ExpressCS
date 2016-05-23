using System.Collections.Generic;
using Microsoft.Owin;
using ExpressFunc = System.Func<ExpressCS.ExpressRequest, ExpressCS.ExpressResponse, System.Func<System.Threading.Tasks.Task>, System.Threading.Tasks.Task>;

namespace ExpressCS
{
    public class RouteHandler : IMiddleware
    {
        public string Path { get; }
        public string Verb { get; }

        public  ExpressFunc Action { get; }

        public RouteHandler(string path, string verb, ExpressFunc action)
        {
            Path = path;
            Verb = verb;
            Action = action;
        }

        public bool Match(IOwinContext context, Router parent)
        {
            var path = context.Request.Path.Value;
            var verb = context.Request.Method;

            return parent.Path + Path.TrimEnd('/') + "/" == path.TrimEnd('/') + "/" && Verb == verb;
        }

        public IEnumerable<RouteHandler> GetStack(IOwinContext context)
        {
            return new[] {this};
        }
    }
}