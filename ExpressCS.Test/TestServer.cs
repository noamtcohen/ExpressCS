using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ExpressCS.Test
{
    public class TestServer : RoutingServer
    {
        public TestServer()
        {
            Get("/", (req, res, next) => res.WriteAsync("Hi!"));

            Get("/no-next", (req, res, next) => next());

            Get("/auth", (req, res, next) => HasAuthenticationCookie(req) ? next() : _403(res));

            Get("/auth", (req, res, next) => res.WriteAsync("You Are Authenticated"));

            Post("/post", (req, res, next) => res.WriteAsync(req.Form["Hi"]));

            Get("/json", (req, res, next) => res.Json(new { Hi = 1 }));

            Post("/jade", (req, res, next) => res.Render(AssemblyDirectory + "/views/test.jade", new { hi = req.Form["Hi"] }));

            Get("/jade-complex", (req, res, next) => res.Render(AssemblyDirectory + "/views/test.complex.jade", new { }));

            Post("/razor", (req, res, next) => res.Render(AssemblyDirectory + "/views/test.cshtml", new { hi = req.Form["Hi"] }));

            var mini = new Router();          
            Use("/mini",mini);
            mini.Get("/site", (req, res, next) => res.WriteAsync("Hi, MiniSite"));

            var minimini = new Router();
            mini.Use("/mm", minimini);
            minimini.Get("/hi", (req, res, next) => res.WriteAsync("minimini"));
        }

        private static Task _403(ExpressResponse res)
        {
            res.StatusCode = 403;
            return res.WriteAsync("");
        }

        private bool HasAuthenticationCookie(ExpressRequest req)
        {
            foreach (var cookie in req.Cookies)
                if (cookie.Key == "i-am-authenticated" && cookie.Value == "hi")
                    return true;

            return false;
        }
        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }
    }
}