using System;
using Xunit;
using ImageRenamer;

namespace ImageRenamer.UnitTests
{
    public class TimestampGeneratorTest
    {
        private readonly string expected = "2018-06-13_15.23.30";

        [Fact]
        public void TestOrigStringShouldReturnExpected()
        {
            string result = TimestampGenerator.Run("2018:06:13 15:23:30");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestImgStringShouldReturnExpected()
        {
            string result = TimestampGenerator.Run("IMG_20180613_152330.jpg");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestBadStringShouldThrow()
        {
            Exception ex = Assert.Throws<TimestampException>(() => TimestampGenerator.Run("blarg"));
            Assert.Equal("String blarg doesn't match any format.", ex.Message);
        }

        [Fact]
        public void TestEmptyStringShouldThrow()
        {
            Exception ex = Assert.Throws<ArgumentException>(() => TimestampGenerator.Run(""));
            Assert.Equal("Can't process empty string.", ex.Message);
        }
    }
}
