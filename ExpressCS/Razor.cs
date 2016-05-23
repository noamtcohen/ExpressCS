using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;
using RazorEngine;
using RazorEngine.Templating;

namespace ExpressCS
{
    internal class Razor : ITemplateEngine
    {
        public Task Render(string path, object model, IOwinResponse _response)
        {
            var templateKey = path;
            var template = File.ReadAllText(path);
            var result = Engine.Razor.RunCompile(template, templateKey, null, model);
            return _response.WriteAsync(result);
        }
    }
}