using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EdgeJs;
using Microsoft.Owin;
using Newtonsoft.Json;
using RazorEngine;

namespace ExpressCS
{
    internal class Jade : ITemplateEngine
    {
        private readonly Dictionary<string,string> _cachedJadeFiles = new Dictionary<string, string>(); 

        private IOwinResponse _response;

        public Task Render(string path, object model,IOwinResponse response)
        {
            _response = response;

            var render = Edge.Func(@" 
                            return function (options, cb) {    
                                
                                try{       
                                    var jade = require('../jade.js');             

                                    if(!GLOBAL.jade_fn)
                                        GLOBAL.jade_fn ={};

                                    if(!GLOBAL.jade_fn[options.path])
                                        GLOBAL.jade_fn[options.path] = jade.compile(options.jade, options.jadeOptions);

                                    var html = GLOBAL.jade_fn[options.path](options.model);
                                    options.onHtml(html);
                                }
                                catch(e){
                                    options.onHtml(e.toString());
                                }

                                cb();
                            };
                        ");

            var onHtml = (Func<object, Task<object>>)(async (message) => Write((string)message));

            if (!_cachedJadeFiles.ContainsKey(path))
                _cachedJadeFiles.Add(path, File.ReadAllText(path));
 
            var opt = new
            {
                onHtml,
                path,
                jade = _cachedJadeFiles[path],
                model,
                jadeOptions = new
                {
                    pretty = true,
                    compileDebug = false,
                    cache = true
                }
            };

            return render(opt);
        }

        public Task Write(string message)
        {
            return _response.WriteAsync(message);
        }
    }
}