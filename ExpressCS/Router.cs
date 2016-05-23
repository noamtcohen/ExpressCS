using System.Collections.Generic;
using Microsoft.Owin;
using ExpressFunc = System.Func<ExpressCS.ExpressRequest, ExpressCS.ExpressResponse, System.Func<System.Threading.Tasks.Task>, System.Threading.Tasks.Task>;

namespace ExpressCS
{
    public class Router : IMiddleware
    {
        protected readonly List<IMiddleware> RouteHanderls = new List<IMiddleware>();

        public string Path { get; private set; }

        public void Get(string path, ExpressFunc action)
        {
            RouteHanderls.Add(new RouteHandler(path,"GET",action));
        }

        public void Post(string path, ExpressFunc action)
        {
            RouteHanderls.Add(new RouteHandler(path,"POST",action));
        }

        public void Use(string path, Router router)
        {
            router.Path = Path + path;
            RouteHanderls.Add(router);
        }

        protected MiddlewareStackHandler GetMiddlewareHandler(IOwinContext context)
        {
            var stack = new MiddlewareStackHandler(context);

            stack.Add(GetStack(context));

            return stack;
        }

        public bool Match(IOwinContext context, Router parent)
        {
            return context.Request.Path.Value.StartsWith(Path);
        }

        public IEnumerable<RouteHandler> GetStack(IOwinContext context)
        {
            var rtn = new List<RouteHandler>();

            foreach (var route in RouteHanderls)
            {
                if (route.Match(context, this))
                    rtn.AddRange(route.GetStack(context));
            }
 
            return rtn;
        }
    }
}