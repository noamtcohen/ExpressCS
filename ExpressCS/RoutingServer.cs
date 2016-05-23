using System;
using Owin;

namespace ExpressCS
{
    public class RoutingServer : Router
    {
        public virtual void Configuration(IAppBuilder app)
        {
            app.Run(context =>
            {
                var handler = GetMiddlewareHandler(context);
                
                return handler.Handle();
            });
        }

        public static IDisposable Start<T>(string uri) where T: RoutingServer
        {
            return Microsoft.Owin.Hosting.WebApp.Start<T>(uri);
        }
    }
}