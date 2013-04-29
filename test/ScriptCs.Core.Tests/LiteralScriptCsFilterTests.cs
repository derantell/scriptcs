using Xunit;

namespace ScriptCs.Tests {
    public class LiteralScriptCsFilterTests 
    {
        public class ShouldHandleMethod 
        {
            [Fact]
            public void ShouldReturnFalseWhenExtensionIsEmpty() {
                var shouldHandle = LiteralScriptCsFilter.ShouldHandle(@"c:\foo\bar");
                Assert.False(shouldHandle);
            }

            [Fact]
            public void ShouldReturnTrueWhenExtenstionIsLitcsx() {
                var shouldHandle = LiteralScriptCsFilter.ShouldHandle(@"c:\foo\bar.litcsx");
                Assert.True(shouldHandle);
            }

            [Fact]
            public void ShouldReturnTrueWhenExtenstionIsMd() {
                var shouldHandle = LiteralScriptCsFilter.ShouldHandle(@"c:\foo\bar.md");
                Assert.True(shouldHandle);
            }

            
            [Fact]
            public void ShouldReturnTrueWhenExtenstionIsMDown() {
                var shouldHandle = LiteralScriptCsFilter.ShouldHandle(@"c:\foo\bar.mdown");
                Assert.True(shouldHandle);
            }

            [Fact]
            public void ShouldReturnTrueWhenExtenstionIsMarkdown() {
                var shouldHandle = LiteralScriptCsFilter.ShouldHandle(@"c:\foo\bar.markdown");
                Assert.True(shouldHandle);
            }


            [Fact]
            public void ShouldReturnFalseWhenExtensionIsOtherThanSupported() {
                var shouldHandle = LiteralScriptCsFilter.ShouldHandle(@"c:\foo\bar.mdo");
                Assert.False(shouldHandle); 
            }
        } 
    }
}