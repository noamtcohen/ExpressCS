using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace ExpressCS
{
    public class ExpressRequest : IOwinRequest
    {
        private readonly IOwinRequest _request;

        public IFormCollection Form { get; }

        public ExpressRequest(IOwinRequest request)
        {
            _request = request;
            Form= request.ReadFormAsync().Result;
        }

        public Task<IFormCollection> ReadFormAsync()
        {
            return _request.ReadFormAsync();
        }

        public T Get<T>(string key)
        {
            return _request.Get<T>(key);
        }

        public IOwinRequest Set<T>(string key, T value)
        {
            return _request.Set<T>(key, value);
        }

        public IDictionary<string, object> Environment => _request.Environment;
        public IOwinContext Context => _request.Context;

        public string Method
        {
            get { return _request.Method; }
            set { _request.Method = value; }
        }

        public string Scheme
        {
            get { return _request.Scheme; }
            set { _request.Scheme = value; }
        }

        public bool IsSecure => _request.IsSecure;

        public HostString Host
        {
            get { return _request.Host; }
            set { _request.Host = value; }
        }

        public PathString PathBase
        {
            get { return _request.PathBase; }
            set { _request.PathBase = value; }
        }

        public PathString Path
        {
            get { return _request.Path; }
            set { _request.Path = value; }
        }

        public QueryString QueryString
        {
            get { return _request.QueryString; }
            set { _request.QueryString = value; }
        }

        public IReadableStringCollection Query => _request.Query;
        public Uri Uri => _request.Uri;

        public string Protocol
        {
            get { return _request.Protocol; }
            set { _request.Protocol = value; }
        }

        public IHeaderDictionary Headers => _request.Headers;
        public RequestCookieCollection Cookies => _request.Cookies;

        public string ContentType
        {
            get { return _request.ContentType; }
            set { _request.ContentType = value; }
        }

        public string CacheControl
        {
            get { return _request.CacheControl; }
            set { _request.CacheControl = value; }
        }

        public string MediaType
        {
            get { return _request.MediaType; }
            set { _request.MediaType = value; }
        }

        public string Accept
        {
            get { return _request.Accept; }
            set { _request.Accept = value; }
        }

        public Stream Body
        {
            get { return _request.Body; }
            set { _request.Body = value; }
        }

        public CancellationToken CallCancelled
        {
            get { return _request.CallCancelled; }
            set { _request.CallCancelled = value; }
        }

        public string LocalIpAddress
        {
            get { return _request.LocalIpAddress; }
            set { _request.LocalIpAddress = value; }
        }

        public int? LocalPort
        {
            get { return _request.LocalPort; }
            set { _request.LocalPort = value; }
        }

        public string RemoteIpAddress
        {
            get { return _request.RemoteIpAddress; }
            set { _request.RemoteIpAddress = value; }
        }

        public int? RemotePort
        {
            get { return _request.RemotePort; }
            set { _request.RemotePort = value; }
        }

        public IPrincipal User
        {
            get { return _request.User; }
            set { _request.User = value; }
        }
    }
}