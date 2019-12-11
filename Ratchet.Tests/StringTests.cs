using Xunit;

namespace Ratchet.Tests {
    public class StringTests {
        [Theory]
        [InlineData("Web.dev.config", "Web.config")]
        [InlineData("Web.DEV.config", "WEB.config")]
        [InlineData("web.BLUE.reston.config", "web.blue.config")]
        public void IsFileDirectlyBasedOn(
            string transform, string source
        ) => Assert.True(transform.IsFileDirectlyBasedOn(source));

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "bar")]
        [InlineData("foo", "")]
        [InlineData("foo", "bar")]
        [InlineData("Web.config", "Web.config")]
        [InlineData("Web.DEV.config.bak", "config.bak")]
        [InlineData("Web.DEV.config.bak", "WEB.config")]
        [InlineData("Web.DEV.blue.config", "WEB.config")]
        [InlineData("web.DEV.config", "web.DEV.blue.config")]
        public void IsNotFileDirectlyBasedOn(
            string transform, string source
        ) => Assert.False(transform.IsFileDirectlyBasedOn(source));

        [Theory]
        [InlineData("Web.DEV.blue.CONFIG", "WEB.config")]
        public void IsFileUltimatelyBasedOn(
            string transform, string source
        ) => Assert.True(transform.IsFileUltimatelyBasedOn(source));

        [Theory]
        [InlineData("Web2.DEV.blue.config", "WEB.config")]
        public void IsNotFileUltimatelyBasedOn(
            string transform, string source
        ) => Assert.False(transform.IsFileUltimatelyBasedOn(source));
    }
}
