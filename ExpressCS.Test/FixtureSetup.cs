using NUnit.Framework;

namespace ExpressCS.Test
{
    [SetUpFixture]
    class FixtureSetup : FixtureSetupWithHost
    {
        public const string URI = "http://localhost:771";

        public override string GetUri()
        {
            return URI;
        }
    }
}
