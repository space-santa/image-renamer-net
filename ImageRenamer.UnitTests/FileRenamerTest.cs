using System;
using Xunit;
using ImageRenamer;
using System.IO;

namespace ImageRenamer.UnitTests
{
    public class FileRenamerTest
    {
        private readonly string expected = "2018-06-13_15.23.30";

        [Fact]
        public void TestEmptyPathShouldThrow()
        {
            Exception ex = Assert.Throws<ArgumentException>(() => FileRenamer.GetNewName(""));
            Assert.Equal("Can't process empty string.", ex.Message);
        }

        [Fact]
        public void TestFileNotFoundShouldThrow()
        {
            Assert.Throws<FileNotFoundException>(() => FileRenamer.GetNewName("idontexist"));
        }

        [Fact]
        public void TestBadStringShouldThrow()
        {
            Exception ex = Assert.Throws<TimestampException>(() => FileRenamer.GetNewName("../../../testdata/blarg"));
            Assert.Equal("String blarg doesn't match any format.", ex.Message);
        }

        [Fact]
        public void TestImgStringShouldReturnExpected()
        {
            string result = FileRenamer.GetNewName("../../../testdata/IMG_20180613_152330.jpg");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestVidStringShouldReturnExpected()
        {
            string result = FileRenamer.GetNewName("../../../testdata/VID_20180613_152330.mp4");
            Assert.Equal(expected, result);
        }
    }
}
