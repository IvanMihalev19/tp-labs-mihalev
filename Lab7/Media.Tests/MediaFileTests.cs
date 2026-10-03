using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Media.Core;

namespace Media.Tests
{
    public class MediaFileTests
    {
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            var file = new MediaFile("/tmp/music/song.mp3", 1024);
            Assert.Equal("song.mp3", file.FileName);
            Assert.Equal(".mp3", file.Extension);
            Assert.Equal(MediaType.Audio, file.Type);
            Assert.Equal(1024, file.SizeBytes);
        }

        [Fact]
        public void Constructor_EmptyPath_Throws()
        {
            Assert.Throws<ArgumentException>(() => new MediaFile("", 0));
        }
    }
}
