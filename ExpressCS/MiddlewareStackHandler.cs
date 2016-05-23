using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace ExpressCS
{
    public class MiddlewareStackHandler
    {
        private readonly IOwinContext _context;
        public readonly List<RouteHandler> Handlers = new List<RouteHandler>();
        private int _currentIndex;
        private readonly ExpressRequest _request;
        private readonly ExpressResponse _response;

        public MiddlewareStackHandler(IOwinContext context)
        {
            _context = context;
            _request = new ExpressRequest(_context.Request);
            _response = new ExpressResponse(_context.Response);
        }

        public Task Handle()
        {
            if(Handlers.Count<=_currentIndex)
                return _404();

            _context.Response.ContentType = "text/html";
            var func = Handlers[_currentIndex].Action;
            return func(_request,_response, Next);
        }

        public Task Next()
        {
            _currentIndex++;
            return Handle();
        }

        public Task _404()
        {
            _context.Response.ContentType = "text/plain";
            _context.Response.StatusCode = 404;
            return _context.Response.WriteAsync("404");
        }

        public void Add(IEnumerable<RouteHandler> actions)
        {
            Handlers.AddRange(actions);
        }
    }
}