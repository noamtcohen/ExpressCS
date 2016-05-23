using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin;
using Newtonsoft.Json;

namespace ExpressCS
{
    public class ExpressResponse : IOwinResponse
    {
        private readonly IOwinResponse _response;

        private Dictionary<string,ITemplateEngine> _engines = new Dictionary<string, ITemplateEngine>(); 
        public ExpressResponse(IOwinResponse response)
        {
            _response = response;

            _engines.Add(".jade",new Jade());
            _engines.Add(".cshtml", new Razor());
        }

        public Task Json(object o)
        {
            _response.ContentType = "text/json";

            return _response.WriteAsync(JsonConvert.SerializeObject(o));
        }

        public Task Render(string path, object model)
        {
            return _engines[Path.GetExtension(path)].Render(path, model, _response);
        }

        public void OnSendingHeaders(Action<object> callback, object state)
        {
            _response.OnSendingHeaders(callback, state);
        }

        public void Redirect(string location)
        {
            _response.Redirect(location);
        }

        public void Write(string text)
        {
            _response.Write(text);
        }

        public void Write(byte[] data)
        {
            _response.Write(data);
        }

        public void Write(byte[] data, int offset, int count)
        {
            _response.Write(data, offset, count);
        }

        public Task WriteAsync(string text)
        {
            return _response.WriteAsync(text);
        }

        public Task WriteAsync(byte[] data)
        {
            return _response.WriteAsync(data);
        }

        public T Get<T>(string key)
        {
            return _response.Get<T>(key);
        }

        public IOwinResponse Set<T>(string key, T value)
        {
            return _response.Set<T>(key, value);
        }

        public IDictionary<string, object> Environment => _response.Environment;
        public IOwinContext Context => _response.Context;

        public int StatusCode
        {
            get { return _response.StatusCode; }
            set { _response.StatusCode = value; }
        }

        public string ReasonPhrase
        {
            get { return _response.ReasonPhrase; }
            set { _response.ReasonPhrase = value; }
        }

        public string Protocol
        {
            get { return _response.Protocol; }
            set { _response.Protocol = value; }
        }

        public IHeaderDictionary Headers => _response.Headers;
        public ResponseCookieCollection Cookies => _response.Cookies;

        public long? ContentLength
        {
            get { return _response.ContentLength; }
            set { _response.ContentLength = value; }
        }

        public string ContentType
        {
            get { return _response.ContentType; }
            set { _response.ContentType = value; }
        }

        public DateTimeOffset? Expires
        {
            get { return _response.Expires; }
            set { _response.Expires = value; }
        }

        public string ETag
        {
            get { return _response.ETag; }
            set { _response.ETag = value; }
        }

        public Stream Body
        {
            get { return _response.Body; }
            set { _response.Body = value; }
        }

        public Task WriteAsync(byte[] data, int offset, int count, CancellationToken token)
        {
            return _response.WriteAsync(data, offset, count, token);
        }

        public Task WriteAsync(byte[] data, CancellationToken token)
        {
            return _response.WriteAsync(data, token);
        }

        public Task WriteAsync(string text, CancellationToken token)
        {
            return _response.WriteAsync(text, token);
        }
    }

    internal interface ITemplateEngine
    {
        Task Render(string path, object model, IOwinResponse response);
    }
}