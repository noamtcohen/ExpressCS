using System;
using NUnit.Framework;

namespace ExpressCS.Test
{
    public abstract class FixtureSetupWithHost
    {
        public abstract string GetUri();

        private IDisposable _host;

        [OneTimeSetUp]
        public void SetUp()
        {
            _host = RoutingServer.Start<TestServer>(GetUri());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _host.Dispose();          
        }
    }
}
