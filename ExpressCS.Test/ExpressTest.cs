using System.Collections.Specialized;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace ExpressCS.Test
{
    [TestFixture]
    class ExpressTest
    {
        private string TEST_URL = FixtureSetup.URI;
        WebClient _wc;
        
        [SetUp]
        public void Setup()
        {
            _wc = new WebClient();
        }

        [TearDown]
        public void Cleanup()
        {
            _wc.Dispose();    
        }

        [Test]
        public void TestDownloadUrl()
        {
            var s = _wc.DownloadString(TEST_URL);
            Assert.AreEqual("Hi!", s);
        }

        [Test]
        public void TestNoNext()
        {
            try
            {
                _wc.DownloadString(TEST_URL + "/no-next");
            }
            catch (WebException e)
            {
                var response = e.Response as HttpWebResponse;
                Assert.AreEqual(404, (int)response.StatusCode);
            }
        }

        [Test]
        public void TestSecureUrlNotAuthenticated()
        {
            try
            {
                _wc.DownloadString(TEST_URL + "/auth");
            }
            catch (WebException e)
            {
                var response = e.Response as HttpWebResponse;
                Assert.AreEqual(403, (int)response.StatusCode);
            }
        }

        [Test]
        public void TestGetToPost()
        {
            try
            {
                _wc.DownloadString(TEST_URL + "/post");
            }
            catch (WebException e)
            {
                var response = e.Response as HttpWebResponse;
                Assert.AreEqual(404, (int)response.StatusCode);
            }
        }

        [Test]
        public void TestPostToGet()
        {
            try
            {
                _wc.UploadValues(TEST_URL, new NameValueCollection());
            }
            catch (WebException e)
            {
                var response = e.Response as HttpWebResponse;
                Assert.AreEqual(404, (int)response.StatusCode);
            }
        }

        [Test]
        public void TestDownloadUrlWithPost()
        {
            var postVars = new NameValueCollection {{"Hi", "Hello"}};

            var s = _wc.UploadValues(TEST_URL + "/post", postVars);
            Assert.AreEqual("Hello", s);
        }

        [Test]
        public void TestSecureUrlWithAuthHeader()
        {
            _wc.Headers.Add("Cookie", "i-am-authenticated=hi");

            var s = _wc.DownloadString(TEST_URL + "/auth");
            Assert.AreEqual("You Are Authenticated", s);
        }

        [Test]
        public void TestJson()
        {
            var s = _wc.DownloadString(TEST_URL + "/json");
            Assert.AreEqual("{\"Hi\":1}", s);
        }

        [TestCase("Hi Jade!")]
        [TestCase("I am Jade, I cached you. Whats up?")]
        public void TestJade(string str)
        {
            var postVars = new NameValueCollection { { "Hi", str } };

            var s = Encoding.UTF8.GetString(_wc.UploadValues(TEST_URL + "/jade", postVars));
            Assert.AreEqual(str, s);
        }

        [TestCase("First")]
        [TestCase("Cached")]
        public void TestJadeComplex(string str)
        {
            var s = _wc.DownloadString(TEST_URL + "/jade-complex");
        }

        [TestCase("Hi Razor!")]
        [TestCase("I am a cached Razor!!!")]
        public void TestRazor(string str)
        {
            var postVars = new NameValueCollection {{"Hi", str } };

            var s = Encoding.UTF8.GetString(_wc.UploadValues(TEST_URL + "/razor", postVars));
            Assert.AreEqual(str, s);
        }

        [Test]
        public void TestMiniSite()
        {
            var s = _wc.DownloadString(TEST_URL + "/mini/site");
            Assert.AreEqual("Hi, MiniSite", s);
        }

        [Test]
        public void TestMiniMiniSite()
        {
            var s = _wc.DownloadString(TEST_URL + "/mini/mm/hi");
            Assert.AreEqual("minimini", s);
        }
    }
}